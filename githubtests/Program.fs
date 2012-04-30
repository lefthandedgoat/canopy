module main

open runner
open canopy
open configuration

before <- fun () -> System.Console.WriteLine("This happens before every test")
suggestions := false
start "firefox"

let github = "https://www.github.com"

test (fun _ ->
    describe "go to github and login"
    url github
    click "a[href='https://github.com/login']"
    on "https://github.com/login"
    "#login_field" << "canopytest"
    "#password" << "password1"
    click "input[value='Log in']"
    on "https://github.com/"
    "#user" == "canopytest")

test (fun _ ->
    describe "find leftahandedgoat user"
    click "a[href='/search']"
    on "https://github.com/search"
    "input.text" << "lefthandedgoat"
    click "button.classy"
    "div.result h2.title a" *= "lefthandedgoat")

test (fun _ ->
    describe "go to canopy project"
    click "a[href='/lefthandedgoat']"
    on "https://github.com/lefthandedgoat"
    click "a[href='/lefthandedgoat/canopy']"
    on "https://github.com/lefthandedgoat/canopy")

test (fun _ ->
    describe "fork it"
    click "a[href='/lefthandedgoat/canopy/fork']"
    on "https://github.com/canopytest/canopy")

test (fun _ ->
    describe "delete fork"
    click "a[href='/canopytest/canopy/admin']"
    on "https://github.com/canopytest/canopy/admin"
    click "div.ejector a.button span"
    click "div.ejector-actions form button.minibutton")

test (fun _ ->
    describe "log out"
    click "#logout")

run ()

System.Console.ReadKey()

quit ()
