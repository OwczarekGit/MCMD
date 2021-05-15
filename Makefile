release: clean restore
	dotnet publish -c Release -r linux-x64 -p:PublishSingleFile=true --self-contained false
	dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
	dotnet publish -c Release -r osx-x64 -p:PublishSingleFile=true --self-contained false
	mkdir build
	mkdir build/linux-x64/
	mkdir build/windows-x64/
	mkdir build/macos-x64/
	cp MCMD/bin/Release/net5.0/linux-x64/publish/MCMD    build/linux-x64/mcmd
	cp MCMD/bin/Release/net5.0/win-x64/publish/MCMD.exe  build/windows-x64/mcmd.exe
	cp MCMD/bin/Release/net5.0/osx-x64/publish/MCMD      build/macos-x64/mcmd

clean:
	rm -rf build
	rm -rf MCMD/bin/
	rm -rf MCMD/obj/

restore:
	dotnet restore
