let userType=null;
async function loaded(){
    await allBooks();
    fetch(`/library/UserCreds`).then(back=>back.text())
    .then(creds=>userType=creds);
}
async function getBook(method,query){
    return fetch(`/library/GetBook/${method}?query=${encodeURIComponent(query)}`)
    .then(response=>response.json())
    .then(data=>{return data})
    .catch(err=>{return null});
}

function showBooks(data){
    let bookshelf=document.getElementById("bookShelf");
    if(data==null||data==0||data.length==0){
        bookshelf.innerHTML=`<center><h2 id="nO">No books found<br>Did you type it right?</h2></center>`;
    }else{
       bookshelf.innerHTML="";
       data.forEach(book => {
           bookshelf.innerHTML+=`
           <div onclick='selectBook("title","${book.title}")' id="book${book.id}" class="bookShow">
               <img src="imgs/default.jpg" alt="bookImage">
               <div class="description">
                 <a class="title">${book.title}</a>
                 <p>${book.author}<br>${book.pubYear}</p>
               </div>
             </div>
           `
       });
    }
}

async function allBooks(){
    await fetch(`/library/Count`).then(response=>response.text()).then(response=>
        document.getElementById("totalBooks").innerText=`There are ${response} books in the library`);
   try{
        await fetch(`/library/ListBooks/all`).then(response=>response.json()).then(response=>showBooks(response));
   }catch{
        showBooks(null);
   }
    
}

async function deleteBook(id){
    let target=await getBook("id",id);
    target=target[0];
    if(confirm(`do you really wish to delete the book:\n"${target.title}"\nthis action is irreversible!`)){
        await Select("close");
        fetch(`/library/rmBook?query=${encodeURIComponent(target.id)}`)//work on response
    }
    document.getElementById(`book${id}`).remove();
}

async function editBook(id){
    await Select("open");
    let book=await getBook("id",id);
    book=book[0];
    let cancelButton=document.getElementById("cancel");
    let editButton=document.getElementById("edit");
    
    editButton.setAttribute("onclick",`saveBook(${id},"edit")`);
    editButton.innerText="Save";
    cancelButton.innerText="Cancel";
    cancelButton.removeAttribute("hidden");
    cancelButton.setAttribute("onclick",`selectBook('id',${id})`);
    document.getElementById("description").innerHTML=`
        <h2 id="ide"># ${book['id']}</h2>
        <h3>Title</h3>
        <input type="text" id="tit">
        <h3>Author</h3>
        <input type="text" id="aut">
        <h3>Publication Year</h3>
        <input type="number" id="pub">
        <h3>Category</h3>
        <input type="text" id="cat">
    `
        document.getElementById("tit").value=book['title'];
        document.getElementById("aut").value=book['author'];
        document.getElementById("pub").value=book['pubYear'];
        document.getElementById("cat").value=book['category'];
}
async function createBook(){
    await Select("open");
    //let book=await getBook("id",id);
    //book=book[0];
    let editButton=document.getElementById("edit");
    editButton.setAttribute("onclick",`saveBook(0,"create")`);
    editButton.innerText="Save";
    document.getElementById("description").innerHTML=`
        <h2># New Book #</h2>
        <h3>Title</h3>
        <input type="text" id="tit">
        <h3>Author</h3>
        <input type="text" id="aut">
        <h3>Publication Year</h3>
        <input type="number" id="pub">
        <h3>Category</h3>
        <input type="text" id="cat">
    `
}

async function saveBook(id,method){
    title=document.getElementById("tit").value;
    author=document.getElementById("aut").value;
    pubyear=document.getElementById("pub").value;
    category=document.getElementById("cat").value;
    if(
        (pubyear==null||pubyear==0||pubyear=="")||
        (category==null||category=="")||
        (author==null||author=="")||
        (title==null||title=="")
    ){
        alert("No field can be empty");
        return;
    }
    let editedBook={
        'id':Number(id),
        'title':title,
        'author':author,
        'pubYear':Number(pubyear),
        'category':category
    };
    //aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
    if(method=="edit"&&id>0){
        let original= await getBook('id',id);
        original=original[0];
        if(!(
            original["title"]==editedBook["title"]&&
            original["author"]==editedBook["author"]&&
            original["pubYear"]==editedBook["pubYear"]&&
            original["category"]==editedBook["category"]
        )){
            await fetch(`/library/Edit`,{
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(editedBook)
            })
        }
        selectBook("id",id);
        return;
    }else if(method=="create"||id<=0){
        await fetch(`/library/Create/book`,{
            method: "POST",
            headers: {"Content-Type": "application/json"},
            body: JSON.stringify(editedBook)
        })
        await Select("close");
        return;
    }else{
        console.log("Invalid method in saveBook()")
        return;
    }
}
async function selectBook(aspect,info){
    await Select("open");
    if(aspect==null||info==null){
        
        return;
    }
    let Hhtml="";
    fetch(`/library/GetBook/${aspect}?query=${encodeURIComponent(info)}`)
    .then(response=>response.json())
    .then(data=>data.forEach(book=>{
        let a="Not Available";
        if(book.available==1){
            a="available";
        }
        if(userType=="manager"){
            Hhtml=`<button id="edit" onclick="editBook(${book.id})">Edit</button><button id="cancel" onclick="deleteBook(${book.id})">Delete</button>`
        }
        let bookshelf=document.getElementById("bookSearch");
        bookshelf.innerHTML=`
        <div class="inspector">
            <div id="description">
                <h2 id="I"># <span>${book.id}</span> </h2>
                <h2 id="T">"<span>${book.title}</span>"</h2>
                <h3 id="AY">by: '<span>${book.author}</span>' in <span id="PY">${book.pubYear}</span></h3>
                <h3 id="C">${book.category}</h3>
                <h4>• ${a}</h4>
            </div>
            ${Hhtml}
            <button id="close" onclick="Select('close')">X</button>
            <img src="imgs/default.jpg" alt="">
        </div>
        `;
    }))
}

async function Select(a){
    let bok=document.getElementById("bookSearch");
        bok.innerHTML=`
        <div class="inspector">
            <div id="description">
                <h2 id="I"># <book id> </h2>
                <h2 id="T"><book title></h2>
                <h3 id="AY">by: <book author> in <span id="PY"><books pub. year></span></h3>
                <h3 id="C"><book category></h3>
                <h4>• <is book available></h4>
            </div>
            <button id="edit">Edit</button>
            <button id="close" onclick="await Select('close')">X</button>
            <img src="imgs/default.jpg" alt="">
        </div>
        `;
        //<button id="cancel">Delete</button>
    let bookshelf=document.getElementById("bookShelf").querySelectorAll("div");
    let bookSearch=document.getElementById("bookSearch");
    if(a=="open"){
        bookshelf.forEach(book=>book.setAttribute("hidden",""));
        bookSearch.removeAttribute("hidden");
    }else if(a=="close"){
        bookSearch.setAttribute("hidden","");
        bookshelf.forEach(book=>book.removeAttribute("hidden"));
        //right here, when selector is closed i need to update the books...
        //loaded() breaks...
    }else{
        console.log("Error: Unknown method to 'select' at function 'Select(a)'")
        return;
    }
}

function update(){
    yearFilter=document.getElementById("filterType").value;
    serch=document.getElementById("searchBar");
    if(yearFilter=="pubYear"){
        serch.setAttribute("placeholder","Type the book's pub. year");
        serch.setAttribute("type","number");
    }else if(yearFilter=="id"){
        serch.setAttribute("type","number");
        serch.setAttribute("placeholder","Type the book's id");
    }else if(yearFilter=="author"){
        serch.setAttribute("placeholder","Type the book author's name");
        serch.setAttribute("type","text");
    }else{
        serch.setAttribute("type","text");
        serch.setAttribute("placeholder","Type the book's name");
    }
}

async function clearSearch(){
    document.getElementById("searchBar").value="";
    await Select("close");
    allBooks();
}

async function search(){
    await Select("close");
    let type=document.getElementById("filterType").value;
    let info=document.getElementById("searchBar").value;
    switch(type){
        case "title":
            if(info==null||info==""){
                alert("Type the name of the book at the search bar");
                return;
            }
        break;
        case "author":
            if(info==null||info==""){
                alert("Type the name of the book's author at the search bar");
                return;
            }
        break;
        case "pubYear":
            year=Number(info);
            if(year==null||year==""){
                alert("Type the year at the search bar");
            }
        break;
        case "id":
            info=Number(info);
            if(info==null||info==""){
                alert("Type the book's id at the search bar");
            }
        break;
        default:
            alert("Stop messing around");
        return;
    }
    let book=await getBook(type,info)
    showBooks(book);
}