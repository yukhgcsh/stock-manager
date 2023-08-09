$distDir = "../../dist"

if (Test-Path $distDir) {
    Remove-Item -Force -Recurse $distDir
}

mkdir $distDir | Out-Null
mkdir $distDir/app | Out-Null

Copy-Item -Path ../docker/* -Recurse $distDir/
Copy-Item -Path ../sql -Recurse $distDir/mysql/
Copy-Item -Path ../../src/* -Recurse $distDir/app
Copy-Item -Path ../../Dockerfile $distDir/app