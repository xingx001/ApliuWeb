<html>
<body>
<?php
include '../../mysql/config.php';
$id=$_GET['id'];
$sql="";
if($id=="gamerecord")$sql="select * from (select Id, username,game,score,data from Gamedata order by data desc limit 10)g order by g.Id asc";
if($id=="GameMax")$sql="(select username,game,score,max,usetime from Gamedata where game='Game2048' order by score desc,data asc limit 1) union
(select username,game,score,max,usetime from Gamedata where game='Flappy2048' order by score desc,data asc limit 1) union
(select username,game,score,max,usetime from Gamedata where game='pins' order by score desc,data asc limit 1);";
if($id=="Gameabout")$sql="select (select count(*) from Gamedata where game='Game2048') plays,(select max(score) from Gamedata where game='Game2048') maxscore UNION
select (select count(*) from Gamedata where game='Flappy2048') plays,(select max(score) from Gamedata where game='Flappy2048') maxscore UNION
select (select count(*) from Gamedata where game='pins') plays,(select max(score) from Gamedata where game='pins') maxscore;";
$con = mysql_connect($connectphp,$dbuserphp,$dbpasswordphp);
if (!$con)
{
  //die('Could not connect: ' . mysql_error());
}
mysql_select_db($dbnamephp, $con);
mysql_query("set character set 'utf8'");//读库
$result = mysql_query($sql);
$tablebody="";
//id=gamerecord <li>Apliu 玩了 Ganme2048，得了 <b>5862</b>分</li>  username,game,score,data
//id=Game2048   <table width="100%" cellpadding="0" cellspacing="0" class="yx"><tr class="tit"><td>会员</td><td>游戏</td><td>Max</td><td>用时</td><td align="right">分数</td></tr></table> username,game,score,max,usetime
//$id=="Game2048about" <p>最高分：<b>2344</b><br/>人气：<b>111</b></p> plays  maxscore
while($row = mysql_fetch_array($result))
{
	if($id=="gamerecord") 
		$tablebody ="<ul class='scorelist'><li>" . $row['username'] . "玩了" . $row['game'] . "，得了 <b>" . $row['score'] . "</b>分 " . $row['data'] . "</li></ul>" . $tablebody;
	if($id=="GameMax"){ //str_replace(" ","",$str)
		$gameother="";
		switch ($row['game'])
		{
		case "Game2048": $gameother="(Max)";break;
		case "Flappy2048": $gameother="(Max)";break;
		case "pins": $gameother="(关)"; $row['game']="见缝插针";break;
		default:break;
		}
		$tablebody ="<table width='100%' cellpadding='0' cellspacing='0'  class='yx'><tr class='tit'><td width='20%'>" . $row['username'] . "</td><td width='25%'>" . str_replace(" ","",$row['game']) . "</td><td width='15%'>" . str_replace(" ","",$row['score']) . "</td><td width='15%'>" . str_replace(" ","",$row['usetime']) . "秒</td><td align='right' width='25%'>" . $row['max'] .$gameother."</td></tr></table>". $tablebody;
	}
	if($id=="Gameabout")
		$tablebody ="<p>最高分：<b>" . $row['maxscore'] . "</b><br/>人气：<b>" . $row['plays'] . "</b></p>" ."|". $tablebody;
}
echo $tablebody;
mysql_close($con);
?>
</body>
</html>