
        async function submit(){    
            let psP= document.getElementById("psP");
            let usP= document.getElementById("usP");
            let usr= document.getElementById("name").value;
            let pas= document.getElementById("password").value;
            if(usr.length==0){
                for(let i=0;i<usr.length;i++){
                    if(usr[i]=="-"||usr[i]=="\""){
                        usr.innerText="Ivalid characters"
                        return;
                }
            }
                usP.innerText="Type your name!"; 
                return;
            }
            if(pas.length==0){
                for(let i=0;i<pas.length;i++){
                    if(pas[i]=="-"||pas[i]=="\""){
                        pas.innerText="Ivalid characters"
                        return;
                }
            }
                psP.innerText="Type the password!";
                return;
            }
            psP.innerText="";
            usP.innerText="";
            fetch(`/library/LogIn?query=${encodeURIComponent(usr)};${encodeURIComponent(pas)}`)
                .then(response=>{if(!response.ok){
                    usP.innerText="Invalid username or password";
                    return response.text().then(err => {console.log(err);});
                }
                window.location.href=response.url;
            }).catch(err=>{console.log(err)});
            
        }
    