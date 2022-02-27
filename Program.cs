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
      Log($"처리 데이터:{i}건", $"소요시간:{sw.ElapsedMilliseconds}ms");

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
      Image baseImage = GetBaseImage();
      Graphics g = Graphics.FromImage(baseImage);
      Image subImage = GetSubImageByAmount(dto.Amount);

      // 이미지 병합
      g.DrawImage(subImage, new Point(0, 0));

      // 글자 쓰기
      g.DrawString(dto.Address, new Font("나눔고딕", 20), new SolidBrush(Color.Red), new Point(0, 70));

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
        filename = "bronz.png";
      }
      else if (100000 > amount)
      {
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
    static Image GetBaseImage()
    {
      return System.Drawing.Image.FromFile($"{GetImagesPath()}/base.png");
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

      if (!System.IO.File.Exists($"{GetImagesPath()}/base.png"))
      {
        Console.WriteLine("base.png not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{GetImagesPath()}/gold.png"))
      {
        Console.WriteLine("gold.png not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{GetImagesPath()}/silver.png"))
      {
        Console.WriteLine("silver.png not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{GetImagesPath()}/bronz.png"))
      {
        Console.WriteLine("bronz.png not found");
        b = false;
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
        dataDto.Address = arr[1];
        dataDto.Amount = int.Parse(arr[2]);
      }

      return dtos;
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
