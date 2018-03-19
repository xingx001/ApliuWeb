<html>
<body>
<?php
include '../../mysql/config.php';
$id=$_GET['id'];
$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
  {
  //die('Could not connect: ' . mysql_error());
  }
//likenum floor
mysql_select_db($dbnamephp, $con);
$sql="select * from Floor where messageid='" . $id ."' order by Id asc";
$result = mysql_query($sql);

$tablebody="";

while($row = mysql_fetch_array($result))
{
	//if($row['username']!=""||$row['username']!=null) $tablebody=$tablebody."-DvZg-UqRm-";
	$tablebody = $tablebody . $row['name'] . "-DvZg-UqRm-" . $row['huifu'] . "-DvZg-UqRm-" . $row['date']."-DvZg-UqRm-";

}
echo $tablebody;
mysql_close($con);
?>
</body>
</html>