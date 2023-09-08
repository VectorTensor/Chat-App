const io = require("socket.io")(3000,{
    cors: {
        origin :["*"],
    },
})

var users = []
var privateUsers = []
io.on('connection',socket => {
    
    console.log("connected")
    privateUsersNames = privateUsers.map(x=> x.name)
    socket.on("GetUsers",()=>{
        socket.emit("UsersList",privateUsersNames)
    })

    

    socket.on("JoinPrivate",(username, callback)=>{
        duplicate = privateUsers.filter(x=> x.name == username )

        if (duplicate.length == 0){

            const user = {
                name : username,
                id : socket.id,
            };
            privateUsers.push(user)
            socket.broadcast.emit("NewClientAdded", user.name)
        

        callback({
            status:"ok"
        })
        }
        

        else{
        callback({
            status:"error"
        }) 
        }
    
    })
    
    socket.on("JoinPublic",(username,callback)=>{
        
        duplicate = users.filter(x=> x.name == username )

        if (duplicate.length == 0){
            const user = {
                name : username,
                id : socket.id,
            };
            users.push(user)
            socket.emit("NewClient",user)
            socket.join("public");
    
    
            callback({
                status:"ok"
            })
        }

        else{
            callback({
                status:"error"
            }) 
        }
        

    })

    socket.on("ohio",(message)=>{
        console.log(message)
       // socket.to("public").emit("NewMessageFromServer",message)
    })

    socket.on("messagePublic",(message)=>{
        console.log(message)
        sendfrom = users.filter(x=>x.id == socket.id)
        
        socket.to("public").emit("NewMessageFromServer",message,sendfrom[0].name)
    })
    

    socket.on("PrivateMessage",(message,toClient,fromClient)=>{
        console.log("privateMessage called for " + toClient)
        //console.log(users)
        sendto = privateUsers.filter(x=>x.name == toClient)
        console.log(sendto)
        socket.to(sendto[0]["id"]).emit("NewPrivateMessage",message,fromClient)
    })

    socket.on("disconnect", () => {
        // socket.rooms.size === 0

        console.log(socket.id + " left the room")
        newUsers = users.filter(x => x.id != socket.id)
        users = newUsers

        newUsers = privateUsers.filter(x => x.id != socket.id)
        privateUsers = newUsers

      });


})


