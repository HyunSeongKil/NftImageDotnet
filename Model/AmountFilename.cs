using System.Collections.Generic;

namespace NftImageDotnet{
  /// <summary>
  /// </summary>
  public class AmountFilename{
    public string ImageFilename{get;set;}
    /// <summary>
    /// 최소금액. include
    /// </summary>
    public int MinAmount{get;set;}

    /// <summary>
    /// 최대금액. exclude
    /// </summary>
    public int MaxAmount{get;set;}

    private IList<AmountFilename> list;

    public void Init()
    {
      this.list = new List<AmountFilename>{
        new AmountFilename{
          ImageFilename = "type-1.png",
          MinAmount = 0,
          MaxAmount = 1
        },
        new AmountFilename{
          ImageFilename = "type-2.png",
          MinAmount = 1,
          MaxAmount = 100000
        },
        new AmountFilename{
          ImageFilename = "type-3.png",
          MinAmount = 100000,
          MaxAmount = 200000
        },
        new AmountFilename{
          ImageFilename = "type-4.png",
          MinAmount = 200000,
          MaxAmount = 500000
        },
        new AmountFilename{
          ImageFilename = "type-5.png",
          MinAmount = 500000,
          MaxAmount = 10000000
        },
        new AmountFilename{
          ImageFilename = "type-6.png",
          MinAmount = 10000000,
          MaxAmount = int.MaxValue
        },
      };
    }


    public string GetImageFilename(int amount){
      foreach(var item in list){
        if(amount >= item.MinAmount && amount < item.MaxAmount){
          return item.ImageFilename;
        }
      }
      return null;
    }



  }
}