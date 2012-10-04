All Credit goes to Frankelstner

Link: http://www.mordorhq.com/showthread.php?2955-OFFICIAL-Battlefield-3-File-Tweaker


Battlefield 3 File Tweaker Requires .NET 4.5.

Instructions:
1) Either create a new cat or open the existing cas.cat. Note that the cat in the Update folder overwrites most of the original entries so it is highly recommended to look there first. Also note that DLC uses sbtoc archives and is affected only peripherally (it might be not affected at all).
2) Modify any value written in bold.
3) Save and activate the cat.

The tool will not write into the original assets (the cas archives) but instead create new cas archives in the range 50-99. 
It will make a copy of the original file to be tweaked in the new cas archive and apply changes there. 
The game uses a cat file containing the file hash, offset, size and archive number for each entry. 
I just tell the cat that the asset to look for (via hash) is not in archive 1-10 but instead in archive 50 so the cat file has to be overwritten to make any changes. 
However, the tool will make a backup of the cat. Also, the information contained is purely redundant as the cas archives themself have all the information to create a 
cat file from scratch (the tool is capable of that too if you delete all cats).To unmod the game just select the appropriate "Restore Cat". 
You can add a new cat in case you don't want to lose all changes when reverting to the unmodded game. Also, if you directly change cas.cat, change a few things so a new c
as archive is created and then restore the cat, that cas archive has just become useless. If you keep doing that you might end up with lots of cas archives that are not 
used anymore so you will probably want to manually delete them. I recommend making a new cat instead of tweaking cas.cat. Basically you alter the new cat when making tweaks 
and when you press "Activate" the new cat will be copied to cas.cat. 


ALL HAIL MORDOR!

(`-')  _                        (`-').-> (`-')  _   _                 <-. (`-')                (`-') _(`-')                 (`-')  
 (OO ).-/    <-.      <-.        (OO )__  (OO ).-/  (_)      <-.          \(OO )_      .->   <-.(OO )( (OO ).->     .->   <-.(OO )  
 / ,---.   ,--. )   ,--. )      ,--. ,'-' / ,---.   ,-(`-'),--. )      ,--./  ,-.)(`-')----. ,------,)\    .'_ (`-')----. ,------,) 
 | \ /`.\  |  (`-') |  (`-')    |  | |  | | \ /`.\  | ( OO)|  (`-')    |   `.'   |( OO).-.  '|   /`. ''`'-..__)( OO).-.  '|   /`. ' 
 '-'|_.' | |  |OO ) |  |OO )    |  `-'  | '-'|_.' | |  |  )|  |OO )    |  |'.'|  |( _) | |  ||  |_.' ||  |  ' |( _) | |  ||  |_.' | 
(|  .-.  |(|  '__ |(|  '__ |    |  .-.  |(|  .-.  |(|  |_/(|  '__ |    |  |   |  | \|  |)|  ||  .   .'|  |  / : \|  |)|  ||  .   .' 
 |  | |  | |     |' |     |'    |  | |  | |  | |  | |  |'->|     |'    |  |   |  |  '  '-'  '|  |\  \ |  '-'  /  '  '-'  '|  |\  \  
 `--' `--' `-----'  `-----'     `--' `--' `--' `--' `--'   `-----'     `--'   `--'   `-----' `--' '--'`------'    `-----' `--' '--' 