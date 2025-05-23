let userType=null;
async function load(){
    await allBooks();
    fetch(`/library/UserCreds`).then(back=>back.text())
    .then(creds=>{userType=creds;
        if(userType=="manager"){
            document.getElementsByTagName("aside")[0].innerHTML+=`
                <button id="create" onclick="createBook()">Create book</button>
                <button id="manageUsr" onclick="manageUsers()">Manage users</button>`;
        }
    });
    document.getElementById("bookSearch").innerHTML=`
        <div class="inspector">
            <div id="description">
                <h2 id="I"># <book id> </h2>
                <h2 id="T"><book title></h2>
                <h3 id="AY">by: <book author> in <span id="PY"><books pub. year></span></h3>
                <h3 id="C"><book category></h3>
                <h4>• <is book available></h4>
            </div>
            <button id="edit">Edit</button>
            <button id="cancel">Delete</button>
            <button id="close" onclick="Select('close')">X</button>
            <img src="imgs/default.jpg" alt="">
        </div>`;
}

async function getBook(method,query){
    return fetch(`/library/GetBook/${method}?query=${encodeURIComponent(query)}`)
    .then(response=>response.json())
    .then(data=>{return data})
    .catch(err=>{return null});
}

async function allBooks(){
    await fetch(`/library/Count`).then(response=>response.text())
    .then(response=>document.getElementById("totalBooks").innerText=`There are ${response} books in the library`);
    try{
        await fetch(`/library/ListBooks/all`)
        .then(response=>response.json())
        .then(response=>showBooks(response));
    }catch{showBooks(null);}
}

async function deleteBook(id){
    let target=await getBook("id",id);
    let message=`do you really wish to delete the book:\n"${target.title}"\nthis action is irreversible!`
    target=target[0];
    if(confirm(message)){
        await Select("close");
        fetch(`/library/rmBook?query=${encodeURIComponent(target.id)}`);
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
    // \/make a function to display this or maybe take input or smth?
    document.getElementById("description").innerHTML=`
        <h2 id="ide"># ${book['id']}</h2>
        <h3>Title</h3>
            <input type="text" id="tit">
        <h3>Author</h3>
            <input type="text" id="aut">
        <h3>Publication Year</h3>
            <input type="number" id="pub">
        <h3>Category</h3>
            <input type="text" id="cat">`;
    document.getElementById("tit").value=book['title'];
    document.getElementById("aut").value=book['author'];
    document.getElementById("pub").value=book['pubYear'];
    document.getElementById("cat").value=book['category'];
}

async function createBook(){
    await Select("open");
    let editButton=document.getElementById("edit");
    let cancelButton=document.getElementById("cancel");
    cancelButton.innerText="Cancel";
    cancelButton.removeAttribute("hidden");
    cancelButton.setAttribute("onclick",`Select("close")`);
    editButton.setAttribute("onclick",`saveBook(0,"create")`);
    editButton.innerText="Save";
    //i can merge both of this and last function into one, right?
    document.getElementById("description").innerHTML=`
        <h2># New Book #</h2>
        <h3>Title</h3>
            <input type="text" id="tit">
        <h3>Author</h3>
            <input type="text" id="aut">
        <h3>Publication Year</h3>
            <input type="number" id="pub">
        <h3>Category</h3>
            <input type="text" id="cat">`;
}

async function saveBook(id,method){
    title=document.getElementById("tit").value;//does this really has to be this big block?
    author=document.getElementById("aut").value;
    pubyear=document.getElementById("pub").value;
    category=document.getElementById("cat").value;
    if(//can i improve this?
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
    if(method=="edit"&&id>0){
        let original= await getBook('id',id);
        original=original[0];
        if(!(
            original["title"]==editedBook["title"]&&
            original["author"]==editedBook["author"]&&
            original["pubYear"]==editedBook["pubYear"]&&
            original["category"]==editedBook["category"]//this all may be useless, idk?
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

async function manageUsers(){
    Select("close")
    document.getElementById("searchDiv").setAttribute("hidden","");
    document.getElementById("menuDiv").setAttribute("hidden","");
    document.getElementById("create").innerText="Create user";
    document.getElementById("create").setAttribute("onclick","newUser()");
    document.getElementById("manageUsr").setAttribute("onclick","clearSearch()");
    document.getElementById("manageUsr").innerText="Show books";
    let bookshelf=document.getElementById("bookShelf");
    await fetch(`library/getUsers/all`)
    .then(response=>response.json())
    .then(data=>{
        if(data==null||data==0||data.length==0){
            bookshelf.innerHTML=`
            <center><h2 id="nO">No users<br>Maybe an error?</h2></center>`;
        }else{
            bookshelf.innerHTML="";
            data.forEach(user => {
                let idk=user.role
                let eh="";
                if(user.role=="manager"){
                    idk=`<span>${user.role}</span>`
                }else{
                    eh=`<button id="rmUser" onclick="removeUser(${user.id})">delete</button>`
                }
                bookshelf.innerHTML+=`
                <div id="user${user.id}" class="userShow">
                    <h2>#${user.id}</h2>
                    <img src="imgs/user.jpg" alt="user image">
                    <div class="Uinfo">
                        <h2 class="name">${user.name}</h2>
                        <p>- ${idk}</p>
                        ${eh}
                    </div>
                </div>`;
           });
        }        
    })
}

async function selectBook(aspect,info){
    await Select("open");
    if(aspect==null||info==null){return;}
    let Hhtml="";
    fetch(`/library/GetBook/${aspect}?query=${encodeURIComponent(info)}`)
    .then(response=>response.json())
    .then(data=>data.forEach(book=>{
        let a="Not Available";
        if(book.available==1){a="available";}
        if(userType=="manager"){
            Hhtml=`
            <button id="edit" onclick="editBook(${book.id})">Edit</button>
            <button id="cancel" onclick="deleteBook(${book.id})">Delete</button>`;
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
        </div>`;
    }))
}

async function Select(a){
    let bookshelf=document.getElementById("bookShelf").querySelectorAll("div");
    let bookSearch=document.getElementById("bookSearch");
    if(a=="open"){
        bookshelf.forEach(book=>book.setAttribute("hidden",""));
        bookSearch.removeAttribute("hidden");
    }else if(a=="close"){
        bookSearch.setAttribute("hidden","");
        bookshelf.forEach(book=>book.removeAttribute("hidden"));
    }else{
        console.log("Error: Unknown method to 'select' at function 'Select(a)'")
        return;
    }
}

async function clearSearch(){
    //i can definetly improve this...
    //right?
    document.getElementById("searchDiv").removeAttribute("hidden");
    document.getElementById("menuDiv").removeAttribute("hidden");
    document.getElementById("bookShelf").innerHTML="";
    document.getElementById("manageUsr").setAttribute("onclick","manageUsers()");
    document.getElementById("manageUsr").innerText="Manage users";
    document.getElementById("create").innerText="Create book";
    document.getElementById("create").setAttribute("onclick","createbook()");
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

function showBooks(data){
    let bookshelf=document.getElementById("bookShelf");
    if(data==null||data==0||data.length==0){
        bookshelf.innerHTML=`
        <center><h2 id="nO">No books found
        <br>
        Did you type it right?</h2></center>`;
    }else{
        bookshelf.innerHTML="";
        data.forEach(book => {
            bookshelf.innerHTML+=`
            <div onclick='selectBook("id","${book.id}")' id="book${book.id}" class="bookShow">
                <img src="imgs/default.jpg" alt="bookImage">
                <div class="description">
                    <a class="title">${book.title}</a>
                    <p>${book.author}<br>${book.pubYear}</p>
                </div>
            </div>`;
        });
    }
}

function update(){
    //can i improve this too maybe?
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
/*
    i can maybe move all the 'document.getElementById()' variables to the
    global scope, its probably cleaner

    also, i need to break this script, its too big
*/