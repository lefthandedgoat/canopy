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
