async function getBook(method,query){
    let met=document.getElementById("order").value;
    let ord=document.getElementById("way").value;
    return fetch(`/library/GetBook/${method}?query=${encodeURIComponent(query)}&method=${encodeURIComponent(met)}&order=${encodeURIComponent(ord)}`)
    .then(response=>response.json())
    .then(data=>{return data})
    .catch(err=>{return null});
}

async function allBooks(){
    let met=document.getElementById("order").value;//do html for this
    let ord=document.getElementById("way").value;//do html for this
    await fetch(`/library/Count`).then(response=>response.text())
    .then(response=>document.getElementById("totalBooks").innerText=`There are ${response} books in the library`);
    try{
        await fetch(`/library/ListBooks/all?method=${encodeURIComponent(met)}&order=${encodeURIComponent(ord)}`)
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
    if(book.available!=1){
        alert("Book must be available to be edited");
        return;
    }
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

async function addNewUser(){
    let newUser;
    let name=document.getElementById("newName").value;
    let role=document.getElementById("newRole").value;
    if(name==""||role==""){
        alert("fields acannot be empty");
        return;
    }
    newUser={
        'name':name,
        'role':role,
    }; 
    await fetch(`/library/Create/user`,{
        method: "POST",
        headers: {"Content-Type": "application/json"},
        body: JSON.stringify(newUser)
    })
    //could improve
}

async function manageUsers(){
    Select("close")
    document.getElementById("searchDiv").setAttribute("hidden","");
    document.getElementById("menuDiv").setAttribute("hidden","");
    document.getElementById("create").setAttribute("hidden","");
    document.getElementById("manageUsr").setAttribute("onclick","clearSearch()");
    document.getElementById("manageUsr").innerText="Show books";
    let bookshelf=document.getElementById("bookShelf");
    bookshelf.innerHTML="";
    bookshelf.innerHTML+=`
        <div id="square" onclick="newUser()" class="userShow">
            <div id="addNewUser">+</div>
        </div>`;
    await fetch(`/library/getUsers/all`)
    .then(response=>response.json())
    .then(data=>{
        if(data==null||data.length==0){
            bookshelf.innerHTML=`
            <center><h2 id="nO">No users<br>Maybe an error?</h2></center>`;
        }else{
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
                    <img src="../imgs/user.jpg" alt="user image">
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

async function removeUser(id){
    let message=`do you really wish to delete the user by id ${id}?\nthis action is irreversible!`
    if(confirm(message)){
        fetch(`/library/rmUser?query=${encodeURIComponent(id)}`);
    }
}

async function selectBook(aspect,info,method="",order=""){
    await Select("open");
    if(aspect==null||info==null){return;}
    let Hhtml="";
    fetch(`/library/GetBook/${aspect}?query=${encodeURIComponent(info)}&method=${encodeURIComponent(method)}&order=${encodeURIComponent(order)}`)
    .then(response=>response.json())
    .then(book=>{
        book=book[0];
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
            <img src="../imgs/default.jpg" alt="">
        </div>`;
    })
}
