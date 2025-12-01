# Docker quickstart — Root README

This file contains a short, focused Docker/Compose quickstart for the repo; see `docs/DOCKER.md` for full detail and CI suggestions.

## What this covers
- How to build and run `ChildAPI` and `ChildApp` images locally
- How to use `docker-compose` to run both services together
- Basic environment variables for LLM configuration

## Build & run
### Build and run the API
```bash
# From repo root
docker build -f ChildAPI/Dockerfile -t local/childapi:latest .
# Run the API container
docker run --rm -it -p 5000:80 --name childapi local/childapi:latest
```

### Build and run the console app
```bash
docker build -f ChildApp/Dockerfile -t local/childapp:latest .
# Run the app (console app will exit once finished)
docker run --rm -it --name childapp --env LLM_ENDPOINT= --env LLM_API_KEY= local/childapp:latest
```

## Use docker-compose
```bash
# Build and run in foreground
docker compose up --build

# Build and run detached
docker compose up -d --build

# Stop and remove containers
docker compose down
```

The `childapi` service is mapped to http://localhost:5000 and environment variables `LLM_ENDPOINT` and `LLM_API_KEY` are available for configuration using `docker run` or `docker-compose`.

## Push images to a registry
```bash
docker tag local/childapi:latest <registry>/childapi:1.0.0
docker push <registry>/childapi:1.0.0

docker tag local/childapp:latest <registry>/childapp:1.0.0
docker push <registry>/childapp:1.0.0
```

## CI suggestions (quick)
- Add a build step in GitHub Actions to build and test the Docker images; optionally add a `release` job to push images to a registry on tag.
- For headless Selenium tests, add a job that runs a Selenium image and executes the tests against `childapi`.

## Reference
Full details and extended instructions are in `docs/DOCKER.md`.
