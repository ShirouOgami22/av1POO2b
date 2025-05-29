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
            <img src="../imgs/default.jpg" alt="">
        </div>`;
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
function newUser(){
    let square=document.getElementById("square");
    square.removeAttribute("onclick");
    square.innerHTML=`
    <input type="text" placeholder="Name" id="newName">
    <input type="text" placeholder="Role" id="newRole">
    <button onclick="addNewUser()">Create</button>
    `
}
async function clearSearch(){
    //i can definetly improve this...
    //right?
    document.getElementById("searchDiv").removeAttribute("hidden");
    document.getElementById("menuDiv").removeAttribute("hidden");
    document.getElementById("bookShelf").innerHTML="";
    document.getElementById("searchBar").value="";

    try{
    document.getElementById("manageUsr").setAttribute("onclick","manageUsers()");
    document.getElementById("manageUsr").innerText="Manage users";
    document.getElementById("create").removeAttribute("hidden");
    }catch(err){console.log(err);}
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
    let book=await getBook(type,info);
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
            <div onclick='selectBook("id",${book.id})' id="book${book.id}" class="bookShow">
                <img src="../imgs/default.jpg" alt="bookImage">
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