
task :default => [:build, :ui]

task :build do
  sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe canopy.sln' 
  #sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe canopy.sln /p:VisualStudioVersion=12.0' 
end

task :ui do
  sh 'tests\basictests\bin\debug\basictests.exe'
end
