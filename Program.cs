using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;

namespace NftImageDotnet
{
  class Program
  {
    static void Main(string[] args)
    {

      if (!Valid())
      {
        return;
      }

      string[] lines = System.IO.File.ReadAllLines($"{Environment.CurrentDirectory}/data.txt");
      List<DataDto> dataDtos = ToDataDtos(lines);
      Log($"전체 데이터:{dataDtos.Count}건");


      Stopwatch sw = new Stopwatch();
      sw.Start();

      int i = 0;
      foreach (DataDto dto in dataDtos)
      {
        Process(dto);

        if (i++ % 100 == 0)
        {
          // 100건마다 로그 표시
          Log($"#{i}", $"{sw.ElapsedMilliseconds}ms");
        }
      }

      sw.Stop();
      Log($"처리 데이터:{i}건", $"소요시간:{sw.ElapsedMilliseconds}ms");

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

      return Image.FromFile($"{Environment.CurrentDirectory}/{filename}");
    }


    /**
     * 기본 이미지 가져오기
    */
    static Image GetBaseImage()
    {
      return System.Drawing.Image.FromFile($"{Environment.CurrentDirectory}/base.png");
    }



    /**
    * 값 검사
    */
    static Boolean Valid()
    {
      bool b = true;

      if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/data.txt"))
      {
        Console.WriteLine("data.txt not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/base.png"))
      {
        Console.WriteLine("base.png not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/gold.png"))
      {
        Console.WriteLine("gold.png not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/silver.png"))
      {
        Console.WriteLine("silver.png not found");
        b = false;
      }

      if (!System.IO.File.Exists($"{Environment.CurrentDirectory}/bronz.png"))
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
