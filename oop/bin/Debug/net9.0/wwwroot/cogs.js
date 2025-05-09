function getBook(query){
    return fetch(`/library/getBook?query=${encodeURIComponent(query)}`)
    .then(response=>response.json());
}
function showBooks(data){
    let bookshelf=document.getElementById("bookShelf");
    if(data.length==0){
        bookshelf.innerHTML=`<center><h2>No books found</h2></center>`;
    }else{
        bookshelf.innerHTML=``;
        data.forEach(book => {
            bookshelf.innerHTML+=`
            <div class="bookShow">
                <img src="imgs/default.jpg" alt="bookImage">
                <div class="description">
                  <a href="thisBooksPageLink?" class="title">${book.title}</a>
                  <p>${book.author}<br>${book.pubyear}</p>
                </div>
              </div>
            `//return a listttttttt helppppp
        }).catch(err=>{
            bookshelf.innerHTML=`<center><h2>Problem loading books<br>Check Log</h2></center>`;
            console.error(err);
        });
    }
}
function allBooks(){
    return fetch(`/library/getBook?query=${encodeURIComponent("all")}`)
    .then(response=>response.json());
}
function loaded(){
    showBooks(allBooks());
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
    yearTweaks=document.getElementById("filterYear");
    serch=document.getElementById("searchBar");
    if(yearFilter=="pubYear"){
        serch.setAttribute("placeholder","Type the book's name");
        yearTweaks.removeAttribute("hidden");
    }else if(yearFilter=="id"){
        serch.setAttribute("type","number");
        serch.setAttribute("placeholder","Type the book's id");
        yearTweaks.setAttribute("hidden","");
        document.getElementById("year").value="";
    }else if(yearFilter=="author"){
        serch.setAttribute("placeholder","Type the book author's name");
        yearTweaks.setAttribute("hidden","");
        document.getElementById("year").value="";
    }else{
        serch.setAttribute("type","text");
        serch.setAttribute("placeholder","Type the book's name");
        yearTweaks.setAttribute("hidden","");
        document.getElementById("year").value="";
    }
}
function clearSearch(){
    document.getElementById("searchBar").value="";
    document.getElementById("year").value="";
}
function search(){
    let type=document.getElementById("filterType").value;
    let info=document.getElementById("searchBar").value;
    let year=document.getElementById("year").value;
    let filt=document.getElementById("yearOptions").value;
    switch(type){
        case "title":
            if(info==null||info==""){
                alert("Type the name of the book at the search bar");
                return;
            }
                //poderia colocar um filtro de input pra segurança e evitar problemas,
                //mas é um trabalho da escola...
        break;
        case "author":
            if(info==null||info==""){
                alert("Type the name of the book's author at the search bar");
                return;
            }
                //poderia colocar um filtro de input pra segurança e evitar problemas,
                //mas é um trabalho da escola...
        break;
        case "pubYear":
            year=Number(year);
            if(year==null||year==""){
                alert("Type the year at the search bar");
            }else{
                if(filt=="at"||filt=="before"||filt=="after"){
                    break;
                }else{return;}
            }
        break;
        case "available":
            year=Number(year);
            if(year==null||year==""){
                alert("Type the year at the search bar");
            }else{
                if(filt=="at"||filt=="before"||filt=="after"){
                    break;
                }else{return;}
            }
        break;
        case "id":
            info=Number(info);
            if(info==null||info==""){
                alert("Type the book's id at the search bar");
            }else{
                //idk, im tired
            }
        break;
        default:
            alert("Stop messing around");
        return;
    }
}
