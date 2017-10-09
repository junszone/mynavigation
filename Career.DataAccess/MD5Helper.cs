/************************************************************
 * 文件： MD5Helper.cs
 * 类名： MD5Helper
 * 作者： 刘周
 * 时间： 2014-04-21
 * 说明： 提供MD5加密功能
 * 版本： 1.0
 * 
 * 历史
 * 版本       内容              修改人         时间
 * 1.0       创建文件           刘周           2014-04-21 
 * 
 ***********************************************************/

/// <summary>
/// MD5加密辅助类
/// </summary>
public static class MD5Helper
{
    /// <summary>
    /// 加密数据
    /// </summary>
    /// <param name="data">要加密的数据</param>
    /// <returns>加密后的字符串</returns>
    public static string Encrypt(string data)
    {
        //获取加密服务  
        System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();

        //获取要加密的字段，并转化为Byte[]数组  
        byte[] pwdBytes = System.Text.Encoding.Unicode.GetBytes(data);

        //加密Byte[]数组  
        byte[] resultEncrypt = md5CSP.ComputeHash(pwdBytes);

        //将加密后的数组转化为字段(普通加密)  
        return System.Convert.ToBase64String(resultEncrypt);
    }
}
