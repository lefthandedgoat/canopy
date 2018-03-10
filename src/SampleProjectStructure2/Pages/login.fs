module login

open canopy
open runner
open canopyExtensions
open config
open common
open home

//page info
let address = root

//selectors
let username = "#UserName"
let password = "#Password"
let login  = "#login_button"

//helpers
let loginAs (user : users) =    
    //function defined in a function, nice f# feature to provide a good API
    //called in pattern matching below
    let login u p =        
        if currentUrl() <> address then url address
        username << u
        password << p
        click login
        on home.Landing

    match user with
    | Administrator -> login "admin" "password!23"    
    | Client -> login "ABCCorp" "somepassword"
    | Manager -> login "Manager" "sljdfsljfslkfj"
    | SalesPerson -> login "salesmanoftheyear" "moneymoneymoney"

let positive _ = 

    context "positive login page tests"

    "login and check welcome text" &&& fun _ ->
        url address
        loginAs Administrator
        welcomeText == "Weclome Administator!"
        
    //more tests

let negative _ =
    
    context "positive login page tests"

    "type bad password and get error" &&& fun _ ->
        url address
        username << "Adminstrator"
        password << "wrongpassword"
        modal == "Wrong password!"

let all _ =
    positive()
    negative()