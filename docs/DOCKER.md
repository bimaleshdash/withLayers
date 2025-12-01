# Dockerizing the repository

This repository includes two containerized example apps: `ChildAPI` (ASP.NET Core Web API) and `ChildApp` (console/demo app). The `Parent/src/MyLib` library is a dependency referenced by both apps.

This document explains how to build and run the images locally, how to use `docker-compose`, and how to set up environment variables for LLM configuration.

## Files added
- `ChildAPI/Dockerfile` — multi-stage Dockerfile to build and publish the Web API.
- `ChildApp/Dockerfile` — multi-stage Dockerfile to build and publish the console app.
- `docker-compose.yml` — compose file to run the API and console app together for development.
- `.dockerignore` — excludes build artifacts from images.

## Build and run the ChildAPI image
To build the `ChildAPI` image and start a container (named "childapi") locally:

```bash
# from the repo root
docker build -f ChildAPI/Dockerfile -t local/childapi:latest .

docker run --rm -it -p 5000:80 --name childapi local/childapi:latest
```

The API will be available at http://localhost:5000/. You can configure `LLM_ENDPOINT` and `LLM_API_KEY` via environment variables passed to `docker run`.

## Build and run the ChildApp image
Use the following to build and run the console app image:

```bash
docker build -f ChildApp/Dockerfile -t local/childapp:latest .

docker run --rm -it --name childapp --env LLM_ENDPOINT= --env LLM_API_KEY= local/childapp:latest
```

Because `ChildApp` is a console app, it will run and then exit. Use `-d` if you want to run it in the background.

## Using docker-compose
Simplest way to run both services and have them talk to each other is with docker-compose:

```bash
# Build and run services
docker compose up --build

# To run in detached mode
docker compose up -d --build

# To stop services
docker compose down
```

Both services have `LLM_ENDPOINT` and `LLM_API_KEY` environment variables available in `docker-compose.yml` — set them if you want real LLM calls.

## Push images to container registry
If you want to push images to a registry (Docker Hub, Azure Container Registry, etc.):

```bash
# Tag images for your registry
docker tag local/childapi:latest <registry>/childapi:1.0.0
docker tag local/childapp:latest <registry>/childapp:1.0.0

# Push
docker push <registry>/childapi:1.0.0
docker push <registry>/childapp:1.0.0
```

## CI integration (suggestion)
- Add a GitHub Actions job to build Docker images as artifacts in the `build` job.
- Optionally add a `release` job to push to the configured registry after a tagged release.

## Notes
- The Dockerfiles run `dotnet publish` for the projects; if you keep `MyLib`'s `GeneratePackageOnBuild` setting the pack step will still produce packages.
- Keep test dependencies out of container runtime images; they should be used only in build/test pipelines.

If you'd like, I can add a CI job snippet to build/publish these Docker images for you and add GitHub Actions secrets for pushing to a private or public registry.