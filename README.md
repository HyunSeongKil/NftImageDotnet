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

- Environment.CurrentDirectory에 base.png, gold.png, silver.png, bronz.png, data.txt 파일존재해야 함
- Environment.CurrentDirectory하위에 out폴더 존재해야 함

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
