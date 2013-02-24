require 'sinatra'
require "sinatra/reloader"
require "json"

get '/' do
  erb :index
end

get '/autocomplete' do
  erb :autocomplete
end

get '/search' do
  sleep 2 
  content_type 'application/json'
  { :results => 
    [
      { :first_name => "Jane", :last_name => "Doe" },
      { :first_name => "John", :last_name => "Smith" }
    ]
  }.to_json
end

get '/readonly' do
  erb :readonly
end

get '/alert' do
  erb :alert
end

get '/waitFor' do
  erb :waitFor
end

get '/noClickTilVisible' do
  erb :noClickTilVisible
end

get '/home' do  
  erb :home
end

get '/sandbox' do
  erb :sandbox
end

get '/doubleClick' do
  erb :doubleClick
end

get '/displayed' do
  erb :displayed
end

get '/notDisplayed' do
  erb :notDisplayed
end

get '/count' do
  erb :count
end

get '/iframe1' do
  erb :iframe1
end

get '/iframe2' do
  erb :iframe2
end