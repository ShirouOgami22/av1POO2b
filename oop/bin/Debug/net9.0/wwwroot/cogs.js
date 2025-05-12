function getBook(method,query){
    fetch(`/library/GetBook/${method}?query=${encodeURIComponent(query)}`)
    .then(response=>response.json())
    .then(data=>{console.log(data);showBooks(data)})
    .catch(err=>showBooks(null));
}

function showBooks(data){
    let bookshelf=document.getElementById("bookShelf");
    if(data==null||data==0){
        bookshelf.innerHTML=`<center><h2 id="nO">No books found<br>Did you type it right?</h2></center>`;
    }else{
       bookshelf.innerHTML="";
       data.forEach(book => {
           bookshelf.innerHTML+=`
           <div class="bookShow">
               <img src="imgs/default.jpg" alt="bookImage">
               <div class="description">
                 <a onclick='selectBook("title","${book.title}")' class="title">${book.title}</a>
                 <p>${book.author}<br>${book.pubYear}</p>
               </div>
             </div>
           `
       });
    }
}
function allBooks(){
    fetch(`/library/Count`).then(response=>response.text()).then(response=>
        document.getElementById("totalBooks").innerText=`There are ${response} books in the library`);
    fetch(`/library/ListBooks/all`).then(response=>response.json()).then(response=>showBooks(response));
    
}
function selectBook(aspect,info){
    fetch(`/library/GetBook/${aspect}?query=${encodeURIComponent(info)}`)
    .then(response=>response.json())
    .then(data=>data.forEach(book=>{
        console.log(
            book['title'],
            book['id'],
            book['available'],
            book['pubYear'],
            book['categor']
        )
        let a="Not Available";
        if(book.available==1){
            a="available";
        }
        let bookshelf=document.getElementById("bookShelf");
        bookshelf.innerHTML="";
        bookshelf.innerHTML=`
        <div class="inspector">
        <div id="description">
        <h2>${book.id} - ${book.title}</h2>
        <h3>${book.author} - ${book.pubYear}</h3>
        <h3>${book.category}</h3>
        <p>${a}</p>
        </div>
        <img src="imgs/default.jpg" alt="">
        </div>
        `;
        
    }))
}
function toggleAdvance(){
    let settings=document.getElementById("extraSearch");
    let hidden=false;
    for(let i=0;i<settings.attributes.length;i++){
        if(settings.attributes[i].name.toString()=="hidden"){
            hidden=true;
            break;
        }
    }
    if(hidden){
        settings.removeAttribute("hidden");
    }else{
        settings.setAttribute("hidden","");
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
    }else{
        serch.setAttribute("type","text");
        serch.setAttribute("placeholder","Type the book's name");
    }
}

function clearSearch(){
    document.getElementById("searchBar").value="";
    allBooks();
}

function search(){
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
    getBook(type,info);
}
