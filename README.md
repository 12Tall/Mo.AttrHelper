# 关于Mo.AttrHelper

## why?  

此项目最主要的目的是为了泛型数据库Helper 打基础，根据模型类及相应的Attribute 注解自动生成sql 语句，至于为什么不用代码生成器？  

  - 个人感觉通过Attribute 更灵活一些。比如同时操作多个对象、更容易结合事务查询 
  - 虽然反射的效率不如写死的字符串，但是灵活性应该可以弥补这一缺陷  
  
## what?  

利用反射获取Attribute时，有一条语句会非常耗时`obj.GetType().GetCustomAttribute<T>()`。
所以就将已经查询过一次的属性记录在一个`Hashtable`里面，下次调用时就不用再通过反射了，就这么简单    

  - **key**: "Attribute全称 class全称( 属性名|字段名|方法名)?"  
  - **value**: Attribute[]  
  
## how?  

使用时就直接用:
```csharp
var a = AttrHelper.GetAttrs<Attribute1>(obj);  // 获取Attribute1[]
var b = AttrHelper.GetAttr<Attribute2>(obj);  // 获取Attribute2，或者null

// 或者其他方法中调用
public void GetAttr()
{
  var properties = GetType().GetProperties();
  foreach (PropertyInfo property in properties)
  {
    var attr = GetAttr<Ignore>(property);
    // ...
  }
}
```
-----  
Maybe if I keep believing my dreams will come to life ...

```txt
  111     222222   TTTTTTTTTTTT            ll  ll  
 1111    22    22   T   TT   T             l l l l  
   11         22        TT                 l l l l  
   11       22          TT       aaaa      l   ll  
   11     22            TT      a    a    ll   l  
   11    22     2       TT      a   aa   l l  ll  l
 111111  22222222       TT       aaa aa    lll lll  
 ```
