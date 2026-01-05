FROM mcr.microsoft.com/dotnet/sdk:8.0

WORKDIR /workspace/src

# Webixビルド用
RUN apt-get update && apt-get install -y nodejs npm

EXPOSE 5000

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:5000"]
