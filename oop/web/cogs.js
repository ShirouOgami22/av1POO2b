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
    getBook(type,info,year,filt);
}

function getBook(){
    
}