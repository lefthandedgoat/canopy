
module OS
  def OS.windows?
    (/cygwin|mswin|mingw|bccwin|wince|emx/ =~ RUBY_PLATFORM) != nil
  end

  def OS.mac?
   (/darwin/ =~ RUBY_PLATFORM) != nil
  end

  def OS.unix?
    !OS.windows?
  end

  def OS.linux?
    OS.unix? and not OS.mac?
  end
end



task :default => [:build, :ui]

task :build do
  if OS.windows? then
    sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe canopy.sln'
    #sh 'C:\Windows\Microsoft.NET\Framework\v4.0.30319\msbuild.exe canopy.sln /p:VisualStudioVersion=12.0'
  end

  if OS.mac? then
    sh 'xbuild canopy.sln'
  end
end

task :ui do
  if OS.windows? then
    sh 'tests\basictests\bin\debug\basictests.exe'
  end

  if OS.mac? then
    sh "mono --debug tests/basictests/bin/debug/basictests.exe"
  end
end
