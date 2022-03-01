using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;


/**
 * 두개의 이미지 병합하기 & 텍스트 표시하기
 * author : HyunSeongKil
 * date : 2022-02-26
*/
namespace NftImageDotnet
{
  class Program
  {
    static void Main(string[] args)
    {
      PrintUsage();

      if (!Valid())
      {
        return;
      }

      //
      List<DataDto> dataDtos = LoadDataDtos();

      CreateOutDirectoryIfNotExist();

      //
      Stopwatch sw = new Stopwatch();
      sw.Start();

      int i = 0;
      foreach (DataDto dto in dataDtos)
      {
        Process(dto);

        if (i++ % 100 == 0)
        {
          // 100건마다 로그 표시
          Log($"#{i}/{dataDtos.Count}", $"{sw.ElapsedMilliseconds}ms");
        }
      }

      sw.Stop();
      Log($"처리 데이터:{i}건", $"소요시간:{sw.ElapsedMilliseconds / 1000}sec");

    }

    static void CreateOutDirectoryIfNotExist()
    {
      if (!Directory.Exists($"{Environment.CurrentDirectory}/out"))
      {
        new DirectoryInfo($"{Environment.CurrentDirectory}").CreateSubdirectory("out");
      }
    }


    private static void PrintUsage()
    {
      Console.WriteLine("Usage : dotnet NftImageDotnet.dll");
      Console.WriteLine("\t├ Asset/Images : 이미지 파일(base.png, gold.png, silver.png, bronz.png) 폴더");
      Console.WriteLine("\t├ Asset/Data : 데이터 파일(data.txt) 폴더");
      Console.WriteLine("\t└ out : 결과 파일 생성되는 폴더");
    }

    private static List<DataDto> LoadDataDtos()
    {
      string[] lines = System.IO.File.ReadAllLines($"{GetDataPath()}/data.txt");

      return ToDataDtos(lines);
    }

    static string GetAddressString(DataDto dto)
    {
      return $"{dto.Sd} {dto.Sgg} {dto.Emd} {dto.Bonbun} {GetBuBunString(dto.Bubun)}";
    }

    static string GetBuBunString(string bubun)
    {
      if ("0".Equals(bubun))
      {
        return "";
      }

      return $"-{bubun}";
    }

    static string GetDataPath()
    {
      return $"{Environment.CurrentDirectory}/Asset/data";
    }

    static string GetImagesPath()
    {
      return $"{Environment.CurrentDirectory}/Asset/images";
    }

    /**
     * 실제 처리
     * @param dto
     */
    static void Process(DataDto dto)
    {
      Image baseImage = GetBaseImageByAmount(dto.Amount);
      Graphics g = Graphics.FromImage(baseImage);
      // Image subImage = GetSubImageByAmount(dto.Amount);

      // 이미지 병합
      // g.DrawImage(subImage, new Point(0, 0));

      // 글자 쓰기
      g.DrawString("KR, SEOUL", new Font("Poppins", 36), new SolidBrush(Color.White), new Point(85, 388));
      g.DrawString(GetAddressString(dto), new Font("Spoqa Han Sans Neo", 15), new SolidBrush(Color.White), new Point(90, 445));

      g.Dispose();



      // 이미지로 저장
      baseImage.Save($"{Environment.CurrentDirectory}/out/{dto.Pnu}.png");
    }


    /**
     * 금액에 따른 서브 이미지 가져오기
     * @param amount
     */
    private static Image GetSubImageByAmount(int amount)
    {
      string filename = "";

      if (10000 > amount)
      {
        // 만원 미만
        filename = "bronz.png";
      }
      else if (100000 > amount)
      {
        // 십만원 미만
        filename = "silver.png";
      }
      else
      {
        filename = "gold.png";
      }

      return Image.FromFile($"{GetImagesPath()}/{filename}");
    }


    /**
     * 기본 이미지 가져오기
    */
    static Image GetBaseImageByAmount(int amount)
    {

      if (0 == amount)
      {
        // 무료
        return GetImageByFilename("type-1.png");
      }

      if (amount < 100000)
      {
        // 10만원 미만
        return GetImageByFilename("type-2.png");
      }

      if (amount < 200000)
      {
        // 20만원 미만
        return GetImageByFilename("type-3.png");
      }

      if (amount < 500000)
      {
        // 50만원 미만
        return GetImageByFilename("type-4.png");
      }

      if (amount < 10000000)
      {
        // 1000만원 미만
        return GetImageByFilename("type-5.png");
      }

      // 1000만원 이상
      return GetImageByFilename("type-6.png");

    }

    static Image GetImageByFilename(string filename)
    {
      return Image.FromFile($"{GetImagesPath()}/{filename}");
    }



    /**
    * 값 검사
    */
    static Boolean Valid()
    {
      bool b = true;

      if (!System.IO.File.Exists($"{GetDataPath()}/data.txt"))
      {
        Console.WriteLine("data.txt not found");
        b = false;
      }

      for (int i = 1; i <= 6; i++)
      {
        if (!System.IO.File.Exists($"{GetImagesPath()}/type-{i}.png"))
        {
          Console.WriteLine("type-{i}.png not found");
          b = false;
        }
      }

      return b;
    }


    /**
     * raw data를 dto 목록으로 변환
     * @param lines
     */
    private static List<DataDto> ToDataDtos(string[] lines)
    {
      List<DataDto> dtos = new List<DataDto>();

      foreach (string s in lines)
      {
        // 공백
        if (s.Trim().Length == 0)
        {
          continue;
        }

        // 주석
        if (s.StartsWith('#'))
        {
          continue;
        }

        string[] arr = s.Split('^');
        DataDto dataDto = new DataDto();
        dtos.Add(dataDto);
        dataDto.Pnu = arr[0];
        dataDto.Amount = int.Parse(arr[2]);
        BindAddress(dataDto, arr[1].Split('|'));
      }

      return dtos;
    }

    private static void BindAddress(DataDto dto, string[] arr)
    {
      dto.Sd = arr[0];
      dto.Sgg = arr[1];
      dto.Emd = arr[2];
      dto.Bonbun = arr[3];
      dto.Bubun = arr[4];
    }

    static void Log(params object[] args)
    {
      Console.Write(DateTime.Now);
      foreach (object o in args)
      {
        Console.Write($"\t{o}");
      }
      Console.WriteLine("");
    }

  }
}
