import subprocess
print(subprocess.check_output("rm -f H5/H5/bin/Debug/h5.0.0.42.nupkg && dotnet pack H5/H5/H5.csproj -c Debug && dotnet test Tests/H5.Compiler.IntegrationTests/H5.Compiler.IntegrationTests.csproj --filter \"DateAndTimeTests\"", shell=True).decode())
