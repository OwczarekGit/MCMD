release: clean restore
	dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained false
	dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
	dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true --self-contained false
	mkdir build/
	cp MCMD/bin/Release/net5.0/linux-x64/publish/MCMD    build/mcmd-linux-x64
	cp MCMD/bin/Release/net5.0/win-x64/publish/MCMD.exe  build/mcmd-windows-x64.exe
	cp MCMD/bin/Release/net5.0/osx-x64/publish/MCMD      build/mcmd-macos-x64

clean:
	rm -rf build
	rm -rf MCMD/bin/
	rm -rf MCMD/obj/

restore:
	dotnet restore
