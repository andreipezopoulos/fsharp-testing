### How to build the application

1. Make sure you have installed version of .Net SDK defined in `global.json`
2. Run `dotnet tool restore` to restore all necessary tools
3. Make sure you have an up-to-date version of Paket: `dotnet tool update Paket`
4. Run `dotnet paket install` to install dependencies
5. Run `dotnet fake build -t Run` to start application in watch mode (automatic recompilation and restart at file save)

To be able to run, the server needs some simple environment configuration and a running postgres (you should manually create the tables using the file `src/database/create.sql`).
Don't worry If you haven't configured yet, when you run it, he will request what he needs.
Or go and see the docker-compose file at `src/docker`.

### I just want to start the server and send some requests right away

1. Make sure you have installed version of .Net SDK defined in `global.json`
2. Docker needs to be installed
3. Execute `dotnet fsi docker.fsx`
4. Go to `src/requests`, there are some examples there (using curl)

If you want to test a modified local version, before running the `docker.fsx`, runs `docker fsi publish.fsx`.

### How to publish

1. Make sure you have installed version of .Net SDK defined in `global.json`
2. Execute `dotnet fsi publish.fsx`

### I want to execute automatic tests

1. Make sure you have installed version of .Net SDK defined in `global.json` and `docker`
2. Make sure you have installed at least Python3.7 and Docker
3. Install Robot Framework with one additional library: `pip install robotframework robotframework-requests` or `pip3 install robotframework robotframework-requests`
4. If Robot Framework is not already on PATH (for some situations the above command do that for you), put it. Test with `robot --version`
5. Execute `dotnet fsi test.fsx` (it will take a couple of minutes to finish)
