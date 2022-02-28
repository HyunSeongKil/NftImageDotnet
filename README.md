# 이미지 병합 + 이미지에 텍스트 쓰기

- author : HyunSeongKil
- date : 2022-02-26

## 사용기술

- .net core
- 이미지 처리
- 텍스트 파일 읽기
- 로그 처리
- vscode

## 실행 조건

- Environment.CurrentDirectory\Asset\Images에 type-1~6.png 파일 존재해야 함
- Environment.CurrentDirectory\Asset\Data에 data.txt 파일 존재해야 함
- Environment.CurrentDirectory\Asset\Font의 파일 글꼴로 설치해야 함

## git

```
// push an existing repository from the command line
git remote add origin https://github.com/HyunSeongKil/NftImageDotnet.git
git branch -M master
git push -u origin master
```

## 실행

```
// NftImageDotnet.csproj 파일이 있는 폴더에서는 아래처럼 실행
> dotnet run

// NftImageDotnet.dll 파일이 있는 폴더에서는 아래처럼 실행
> dotnet NftImageDotnet.dll
```

## 실행 시간(예상)

- 약 2.5초 / 100건 at i5-6600 8GB
