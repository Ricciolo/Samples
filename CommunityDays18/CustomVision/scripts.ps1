# build image
docker build -t cdays18-customvision .
# stop all
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
# run image
docker run -p 127.0.0.1:80:80 -d cdays18-customvision