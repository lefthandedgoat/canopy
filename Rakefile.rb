
task :default => [:build, :ui_tests]

task :build do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe canopy.sln'
end

task :server do
  sh "start ruby testpages/app.rb"
end

task :ui_tests do
  sh 'basictests\bin\debug\basictests.exe'
end
