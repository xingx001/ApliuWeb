<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>无标题文档</title>
</head>
<?php 

//配置文件数据值获取。 
//默认没有第三个参数时，按照字符串读取提取''中或""中的内容 
//如果有第三个参数时为int时按照数字int处理。
//如果不存在则返回第一个 
function getconfig($file, $ini, $type="string") 
{ 
if ($type=="int") 
{ 
$str = file_get_contents($file); 
$config = preg_match("/" . $ini . "=(.*);/", $str, $res); 
Return $res[1]; 
} 
else 
{ 
$str = file_get_contents($file); 
$config = preg_match("/" . $ini . "=\"(.*)\";/", $str, $res); 
if($res[1]==null) 
{ 
$config = preg_match("/" . $ini . "='(.*)';/", $str, $res); 
} 
Return $res[1]; 
} 
} 

//配置文件数据项更新 
//默认没有第四个参数时，按照字符串读取提取''中或""中的内容 
//如果有第四个参数时为int时按照数字int处理。 
function updateconfig($file, $ini, $value,$type="string") 
{ 
$str = file_get_contents($file); 
$str2=""; 
if($type=="int") 
{ 
$str2 = preg_replace("/" . $ini . "=(.*);/", $ini . "=" . $value . ";", $str); 
} 
else 
{ 
$str2 = preg_replace("/" . $ini . "=(.*);/", $ini . "=\"" . $value . "\";",$str); 
} 
file_put_contents($file, $str2); 
} 

function dbconnect()
{
	//$connect=getconfig("config.php", "connectip") . ":" . getconfig("config.php", "port");
	$connect="23.234.21.198:3306";
	Return $connect;
}
function dbuser()
{
	//$user=getconfig("config.php", "dbuser");
	$user="a0910141853";
	Return $user;
}
function dbpassword()
{
	//$password=getconfig("config.php", "dbpassword");
	$password="323440";
	Return $password;
}
function dbname()
{
	//$name=getconfig("config.php", "dbname");
	$name="a0910141853";
	Return $name;
}
//updateconfig("config.php", "$name", "admin222222"); 
//echo getconfig("config.php", "connectip");
?>
</html>