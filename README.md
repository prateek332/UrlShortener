# Url Shortening API

### To run this project you must have dotnet-sdk v6.0 installed in your system.

<br />

### Run this project by: -
```
git clone <this repo>
cd UrlShortner-InfraCloud
dotnet run
```

### Api Endpoints

#### POST `/api/shorten-url`
- This endpoint requires a json request body, for e.g. : -
```
{
  "urlToShorten": "https://youtube.com/my-channel/asljdfjsadflj1293"
}
```
- api response is json, for e.g.: -
```
{
  "shortenUrl": "https://server-link/pNVueIq"
}
```
- If user attempts to visit the shortened url, they are redirected to the complete url which is cached at the server.

### Docker Hub Image
- https://hub.docker.com/r/prateek332/url-shortener
- to pull the image, use `docker pull prateek332/url-shortener` (Docker must already  be installed)
- request to localhost docker container are made on port 5000, e.g. `http://localhost:5000/api` (request to this endpoint would return a welcome message)
