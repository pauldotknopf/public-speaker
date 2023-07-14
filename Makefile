all : clean restore build publish docker

clean:
	dotnet clean
	rm -r output

restore:
	dotnet restore

build: 
	dotnet build

publish:
	dotnet publish -c Release -o ./output/bin
	cp Dockerfile ./output

docker:
	docker build -t pauldotknopf/public-speaker ./output

run:
	dotnet run