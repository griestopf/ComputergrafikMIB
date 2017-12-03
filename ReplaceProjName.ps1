# Searches recursively through directories and renames the 'old' phrase to the 'new' 
# both, in file contents (solutions, projects and c# files) as well as in file names
#
# usage
# 
#  .\ReplaceProjName.ps1 -dir .\<directoryname> -old <searchphrase> -new <replacementphrase>

param(
    [string]$dir, # The directory where to start the rename
    [string]$old, # The string to find in file names and in file contents
    [string]$new  # The string to replace
)

# RENAME IN FILENAMES
Get-ChildItem -Path $dir\*  -Include *.cs, *.csproj, *.sln, *.xml  -Recurse |
    ForEach {
        Rename-Item -Path $_.PSPath -NewName $_.Name.replace($old,$new)
    }

# SEARCH AND REPLACE IN FILES
Get-ChildItem -Path $dir\*  -Include *.cs, *.csproj, *.sln, *.xml -Recurse |
    ForEach {
        If (Get-Content $_.FullName | Select-String -Pattern $old) 
        {
            (Get-Content $_ | ForEach {$_ -replace $old, $new}) | Set-Content $_ 
        }
    }

