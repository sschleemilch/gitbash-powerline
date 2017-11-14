# **Windows Git-Bash POWERLINE**

-----------------------

1. [About](#about)
2. [Setup](#setup)
3. [Impressions](#impressions)

-----------------------

## <a name="about">About</a>
Motivation was to write a Git-Bash prompt extension to show even more usefull information for the daily usage of developers. Where the Out-Of-The-Box Git-Bash "only" covers displaying the current branch, this extension should provide these additional information:

* Current working directory (shortened if necessary to 4 path entries)
* Exit-Code of the last command if failed
* Name of the current branch (if on any)
* State of the current branch (color + symbols)
	* Modified
	* Untracked
	* Stashed 
* State of the current repository to origin (colors + numbers + symbols)
	* Ahead
	* Behind
	* Diverged
* Information about detached HEAD
	* detached commit
	* DANGER information when on detached HEAD and dirty/untracked/staged content
* Information when in merge state

-----------------------

## <a name="setup">Setup</a>

The extension was written in C# with the use of the .NET framework. It takes care of parsing the necessary information out of git commands and returning a formatted string to be put directly in the bash prompt variable "PS1".

### .bashrc

Therefore, you will need to adapt your .bashrc with this part in the end:
```shell
function _update_ps1() {
   PS1="$(path/to/gitbash-powerline.exe $? 2> /dev/null)"
}

if [ "$TERM" != "linux" ]; then
   PROMPT_COMMAND="_update_ps1; $PROMPT_COMMAND"
fi
```
It will call the powerline.exe executable everytime a command was entered into the bash console (not when just hitting Enter though). The only parameter is the state of the last command executed. You should find your .bashrc in the content of your HOME variable. You can check it inside of your Git Bash with "echo $HOME". Same goes for .minttyrc of the next section 

### .minttyrc (optional)

Although it is not necessary to change your .minttyrc to get the extension working, I would recommend you to. At least you need to set a font that is able to support the used icons. Also the used colors were adapted to the used color scheme. My used .minttyrc configuration is printed below. You can find the used font in the setup/ folder.

```shell
BoldAsFont=yes
Font=DejaVu Sans Mono for Powerline
BoldAsColour=yes
FontHeight=13
ForegroundColour=248,248,242
BackgroundColour=40,42,54
Black=0,0,0
BoldBlack=40,42,53
Red=255,85,85
BoldRed=255,110,103
Green=80,250,123
BoldGreen=90,247,142
Yellow=241,250,140
BoldYellow=244,249,157
Blue=202,169,250
BoldBlue=202,169,250
Magenta=255,121,198
BoldMagenta=255,146,208
Cyan=139,233,253
BoldCyan=154,237,254
White=191,191,191
BoldWhite=230,230,230
Term=xterm-256color
Transparency=off
OpaqueWhenFocused=no
```

-----------------------