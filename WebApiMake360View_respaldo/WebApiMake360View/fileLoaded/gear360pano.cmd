:<<"::IGNORE_THIS_LINE"
@echo off
goto :CMDSCRIPT
::IGNORE_THIS_LINE

# This is a small script to stitch panorama images produced  by Samsung Gear360
#
# Trick with Win/Linux from here:
# http://stackoverflow.com/questions/17510688/single-script-to-run-in-both-windows-batch-and-linux-bash

################################ Linux part here

# http://stackoverflow.com/questions/59895/can-a-bash-script-tell-which-directory-it-is-stored-in
DIR=$(dirname `which $0`)
SCRIPTNAME=$0
OUTDIR=data
OUTTMPNAME="out"
PTOTMPL_SM_C200="$DIR/gear360sm-c210.pto"
PTOTMPL_SM_R210="$DIR/gear360sm-r210.pto"
JPGQUALITY=97
PTOJPGFILENAME="dummy.jpg"
GALLERYDIR="html"
# Note, this file is inside GALLERYDIR
GALLERYFILELIST="filelist.txt"

# Clean-up function
clean_up() {
  if [ -d "$TEMPDIR" ]; then
    rm -rf "$TEMPDIR"
  fi
}

# Function to check if a command fails, arguments:
# - command to execute
# Source:
# http://stackoverflow.com/questions/5195607/checking-bash-exit-status-of-several-commands-efficiently
run_command() {
  "$@"
  local status=$?
  if [ $status -ne 0 ]; then
    echo "Error while running $1" >&2
    if [ $1 != "notify-send" ]; then
      # Display error in a nice graphical popup if available
      run_command notify-send -a $SCRIPTNAME "Error while running $1"
    fi
    clean_up
    exit 1
  fi
  return $status
}

# Function that processes panorama, arguments:
# - input filename
# - output filename
# - template filename
process_panorama() {
  # Create temporary directory locally to stay compatible with other OSes
  # Not using '-p .' might cause some problems on non-unix systems (cygwin?)
  TEMPDIR=`mktemp -d`

  # Stitch panorama (same file twice as input)
  echo "Processing input images (nona)"
  # We need to use run_command with many parameters, or $1 doesn't get
  # quoted correctly and we cannot use filenames with spaces
  run_command  "nona" "-o" "$TEMPDIR/$OUTTMPNAME" \
               "-m" "TIFF_m" \
               "-z" "LZW" \
               "$3" \
               "$1" \
               "$1"

  echo "Stitching input images (enblend)"
  # We need to use run_command with many parameters,
  # or the directories don't get quoted correctly
  run_command "enblend" "-o" "$2" \
              "--compression=jpeg:$JPGQUALITY" \
              "$TEMPDIR/${OUTTMPNAME}0000.tif" \
              "$TEMPDIR/${OUTTMPNAME}0001.tif"

  # TODO: not sure about the tag exclusion list...
  # Note: there's no check as exiftool is needed by Hugin
  #IMG_WIDTH=7776
  #IMG_HEIGHT=3888
  echo "Setting EXIF data (exiftool)"
  run_command "exiftool" "-ProjectionType=equirectangular" \
              "-q" \
              "-m" \
              "-TagsFromFile" "$1" \
              "-exif:all" \
              "-ExifByteOrder=II" \
              "-FullPanoWidthPixels=$IMG_WIDTH" \
              "-FullPanoHeightPixels=$IMG_HEIGHT" \
              "-CroppedAreaImageWidthPixels=$IMG_WIDTH" \
              "-CroppedAreaImageHeightPixels=$IMG_HEIGHT" \
              "-CroppedAreaLeftPixels=0" \
              "-CroppedAreaTopPixels=0" \
              "--FocalLength" \
              "--FieldOfView" \
              "--ThumbnailImage" \
              "--PreviewImage" \
              "--EncodingProcess" \
              "--YCbCrSubSampling" \
              "--Compression" \
              "$2"

  # Problems with "-delete_original!", manually remove the file
  rm ${2}_original

  # Clean up any files/directories we created on the way
  clean_up
}

print_help() {
  echo -e "\nSmall script to stitch raw panorama files."
  echo "Raw meaning two fisheye images side by side."
  echo -e "Script originally writen for Samsung Gear 360.\n"
  echo -e "Usage:\n$0 [-q quality] [-o outdir] [-h] infile [hugintemplate]\n"
  echo "Where infile is a panorama file from camera, it can"
  echo -e "be a wildcard (ex. *.JPG). hugintemplate is optional.\n"
  echo "Panorama file will be written to a file with appended _pano,"
  echo -e "example: 360_010.JPG -> 360_010_pano.JPG\n"
  echo "-q|--quality will set the JPEG quality to quality"
  echo "-o|--output  will set the output directory of panoramas"
  echo "             default: current directory"
  echo "-g|--gallery update gallery file list, best with"
  echo "             -o html/data"
  echo "-h|--help    prints help"
}

create_gallery() {
  GALLERYFILELISTFULL="${GALLERYDIR}/${GALLERYFILELIST}"
  echo "Updating gallery file list in ${GALLERYFILELISTFULL}"
  ls -l *.mp4 *_pano.jpg > ${GALLERYFILELISTFULL}
}

# Source (modified)
# https://stackoverflow.com/questions/192249/how-do-i-parse-command-line-arguments-in-bash
while [[ $# -gt 0 ]]
do
key="$1"

case $key in
  -q|--quality)
    JPGQUALITY="$2"
    # Two shifts because there's no shift in the loop
    # otherwise we can't handle just "-h" option
    shift
    shift
    ;;
  -h|--help)
    print_help
    shift
    exit 0
    ;;
  -o|--output)
    OUTDIR="$2"
    shift
    shift
    ;;
  -g|--gallery)
    CREATEGALLERY=yes
    shift
    ;;
  *)
    break
    ;;
esac
done

# Check argument(s)
if [ -z "$1" ]; then
  print_help
  exit 1
fi

# Check if we have the software to do it (Hugin, ImageMagick)
# http://stackoverflow.com/questions/592620/check-if-a-program-exists-from-a-bash-script
type nona >/dev/null 2>&1 || { echo >&2 "Hugin required but it's not installed. Aborting."; exit 1; }

STARTTS=`date +%s`

# Warn early about the gallery
if [ "$CREATEGALLERY" == "yes" ] && [ "$OUTDIR" != "data" ] && [ "$OUTDIR" != "./data" ]; then
  echo -e "\nGallery file list will be updated but output directory not set to data\n"
fi

for i in $1
do
  # Detect camera model
  CAMERAMODEL=`exiftool -s -s -s -Model $i`
  case $CAMERAMODEL in
    SM-C200)
      PTOTMPL=$PTOTMPL_SM_C200
      ;;
    SM-R210)
      PTOTMPL=$PTOTMPL_SM_R210
      ;;
    *)
      PTOTMPL=$PTOTMPL_SM_C200
      ;;
  esac
  OUTNAMEPROTO=`dirname "$i"`/`basename "${i%.*}"`_pano.jpg
  OUTNAME=`basename $OUTNAMEPROTO`
  echo "Processing file: $i"
  process_panorama $i $OUTDIR/$OUTNAME $PTOTMPL
done

if [ "$CREATEGALLERY" == "yes" ]; then
  # This could be a bit more elegant, but this is the easiest
  cd $GALLERYDIR
  COUNT=`cat $GALLERYFILELIST | wc -l`
  echo "Updating gallery file list, old file count: $COUNT"
  find data -type f -iname "*.jpg" -o -iname "*.jpeg" -o -iname "*.mp4" > $GALLERYFILELIST
  COUNT=`cat $GALLERYFILELIST | wc -l`
  echo "New file count: $COUNT"
  cd ..
fi

# Inform user about the result
ENDTS=`date +%s`
RUNTIME=$((ENDTS-STARTTS))
echo "Processing took: $RUNTIME s"
echo "Processed files should be in $OUTDIR"

# Uncomment this if you don't do videos; otherwise, it is quite annoying
#notify-send "Panorama written to $OUTNAME, took: $RUNTIME s"
exit 0

################################ Windows part here

:CMDSCRIPT

rem http://stackoverflow.com/questions/673523/how-to-measure-execution-time-of-command-in-windows-command-line
set start=%time%

set HUGINPATH1=C:/Program Files/Hugin/bin
set HUGINPATH2=C:/Program Files (x86)/Hugin/bin
set GALLERYDIR=html
set GALLERYFILELIST=filelist.txt

rem This is to avoid some weird bug (???) %~dp0 doesn't work in a loop (effect of shift?)
set SCRIPTNAME=%0
set SCRIPTPATH=%~dp0
set OUTTMPNAME=out
set PTOTMPL_SM_C200="%SCRIPTPATH%gear360sm-c200.pto"
set PTOTMPL_SM_R210="%SCRIPTPATH%gear360sm-r210.pto"
set INNAME=
set PTOTMPL=
#folder source
set OUTDIR=data
set JPGQUALITY=97
set PTOJPGFILENAME=dummy.jpg

rem Process arguments
set PARAMCOUNT=0
rem We need this due to stupid parameter substitution
setlocal enabledelayedexpansion
:PARAMLOOP
rem Small hack as substring doesn't work on %1 (need to use delayed sub.?)
set _TMP=%1
set FIRSTCHAR=%_TMP:~0,1%
if "%_TMP%" == "" goto PARAMDONE
if "%FIRSTCHAR%" == "/" (
  set SWITCH=!_TMP:~1,2!
  rem Switch processing
  if /i "!SWITCH!" == "q" (
    shift
    rem shift has no effect (delayed expansion not working on %1?) we have to use %2
    set JPGQUALITY=%2
  )
  if /i "!SWITCH!" == "h" (
    goto NOARGS
  )
  if /i "!SWITCH!" == "o" (
    shift
    set OUTDIR=%2
  )
  if /i "!SWITCH!" == "g" (
    set CREATEGALLERY=yes
  )
) else (
  if %PARAMCOUNT% EQU 0 set PROTOINNAME=%_TMP%
  if %PARAMCOUNT% EQU 1 set PTOTMPL=%_TMP%
  set /a PARAMCOUNT+=1
)
shift & goto PARAMLOOP
:PARAMDONE

rem Check arguments and assume defaults
if "%PROTOINNAME%" == "" goto NOARGS
rem OUTNAME will be calculated dynamically
if "%PTOTMPL%" == "" (
  set PTOTMPL=%PTOTMPL_SM_C200%
)

rem Where's enblend? Prefer 64 bits
rem Haha, weird bug, it doesn't work when using brackets (spaces in path)
if exist "%HUGINPATH1%/enblend.exe" goto HUGINOK
rem 64 bits not found? Check x86
if not exist "%HUGINPATH2%/enblend.exe" goto NOHUGIN
rem Found x86, overwrite original path
set HUGINPATH1=%HUGINPATH2%
:HUGINOK

rem Warn early about the gallery
if "%CREATEGALLERY%" == "yes" if not "%OUTDIR%" == "html\data" (
  if /i not "%OUTDIR%" == "html\data" (
    echo.
    echo Gallery file list will be updated but output directory is not set to html\data
    echo.
  )
)

for %%f in (%PROTOINNAME%) do (
  set OUTNAME=%OUTDIR%\%%~nf_pano.jpg
  set INNAME=%%f

  "%HUGINPATH1%/exiftool.exe" -s -s -s -Model !INNAME! > modelname.tmp
  set /p MODELNAME=<modelname.tmp
  del modelname.tmp
  if "!MODELNAME!" == "SM-C200" set PTOTMPL=%PTOTMPL_SM_C200%
  if "!MODELNAME!" == "SM-R210" set PTOTMPL=%PTOTMPL_SM_R210%

  echo Processing file: !INNAME!
  call :PROCESSPANORAMA !INNAME! !OUTNAME! !PTOTMPL!
)

if "%CREATEGALLERY%" == "yes" (
  rem This could be a bit more elegant, but this is the easiest
  cd $GALLERYDIR
  echo Updating gallery file list
  rem Yep, repetition...
  rem https://superuser.com/questions/1029558/list-files-in-a-subdirectory-and-get-relative-paths-only-with-windows-command-li
  for %%X IN ('data') DO FOR /F "TOKENS=*" %%F IN (
    'dir /B /A-D ".\%%~X\*.jpg"'
  ) do echo .\%%~X\%%~F > "%GALLERYFILELIST%"
  for %%X IN ('data') DO FOR /F "TOKENS=*" %%F IN (
    'dir /B /A-D ".\%%~X\*.jpeg"'
  ) do echo .\%%~X\%%~F >> "%GALLERYFILELIST%"
  for %%X IN ('data') DO FOR /F "TOKENS=*" %%F IN (
    'dir /B /A-D ".\%%~X\*.mp4"'
  ) do echo .\%%~X\%%~F >> "%GALLERYFILELIST%"
  cd ..
)

rem Time calculation
set end=%time%
set options="tokens=1-4 delims=:.,"
rem Don't try to break lines here, it will most probably not work
for /f %options% %%a in ("%start%") do set start_h=%%a&set /a start_m=100%%b %% 100&set /a start_s=100%%c %% 100&set /a start_ms=100%%d %% 100
for /f %options% %%a in ("%end%") do set end_h=%%a&set /a end_m=100%%b %% 100&set /a end_s=100%%c %% 100&set /a end_ms=100%%d %% 100

set /a hours=%end_h%-%start_h%
set /a mins=%end_m%-%start_m%
set /a secs=%end_s%-%start_s%
set /a ms=%end_ms%-%start_ms%
if %ms% lss 0 set /a secs = %secs% - 1 & set /a ms = 100%ms%
if %secs% lss 0 set /a mins = %mins% - 1 & set /a secs = 60%secs%
if %mins% lss 0 set /a hours = %hours% - 1 & set /a mins = 60%mins%
if %hours% lss 0 set /a hours = 24%hours%
if 1%ms% lss 100 set ms=0%ms%

rem mission accomplished
set /a totalsecs = %hours%*3600 + %mins%*60 + %secs%

echo Processing took: %totalsecs% s
echo Processed files should be in %OUTDIR%

goto eof

:NOARGS

echo.
echo Script to stitch raw panorama files.
echo Raw meaning two fisheye images side by side.
echo Script originally writen for Samsung Gear 360.
echo.
echo Usage:
echo %SCRIPTNAME% [/q quality] [/o outdir] [/h] infile [hugintemplate]
echo.
echo Where infile is a panorama file from camera, it can
echo be a wildcard (ex. *.JPG). hugintemplate is optional.
echo.
echo Panorama will be written to a file with appended _pano,
echo example: 360_010.JPG -> 360_010_pano.JPG
echo.
echo /q sets output jpeg quality
echo /o sets output directory for stitched panoramas
echo    default: current directory
echo /g update gallery file list, best with
echo    /o html\data
echo /h prints help
echo.
goto eof

:NOHUGIN

echo.
echo Hugin is not installed or installed in non-standard directory
echo Was looking in: %HUGINPATH1%
echo and: %HUGINPATH2%
goto eof

:NONAERROR

echo nona failed, panorama not created
goto eof

:ENBLENDERROR

echo enblend failed, panorama not created
goto eof

:PROCESSPANORAMA

set LOCALINNAME=%1
set LOCALOUTNAME=%2
set LOCALPTOTMPL=%3

rem Execute commands (as simple as it is)
echo Processing input images (nona)
"%HUGINPATH1%/nona.exe" -o %TEMP%/%OUTTMPNAME% ^
              -m TIFF_m ^
              -z LZW ^
              %LOCALPTOTMPL% ^
              %LOCALINNAME% ^
              "%LOCALINNAME%"
if %ERRORLEVEL% equ 1 goto NONAERROR

echo Stitching input images (enblend)
"%HUGINPATH1%/enblend.exe" -o %2 ^
              --compression=jpeg:%JPGQUALITY% ^
              %TEMP%/%OUTTMPNAME%0000.tif ^
              %TEMP%/%OUTTMPNAME%0001.tif
if %ERRORLEVEL% equ 1 goto ENBLENDERROR

rem Check if we have exiftool...
echo Setting EXIF data (exiftool)
set IMG_WIDTH=7776
set IMG_HEIGHT=3888
"%HUGINPATH1%/exiftool.exe" -ProjectionType=equirectangular ^
                            -m ^
                            -q ^
                            -TagsFromFile "%LOCALINNAME%" ^
                            -exif:all ^
                            -ExifByteOrder=II ^
                            -FullPanoWidthPixels=%IMG_WIDTH% ^
                            -FullPanoHeightPixels=%IMG_HEIGHT% ^
                            -CroppedAreaImageWidthPixels=%IMG_WIDTH% ^
                            -CroppedAreaImageHeightPixels=%IMG_HEIGHT% ^
                            -CroppedAreaLeftPixels=0 ^
                            -CroppedAreaTopPixels=0 ^
                            --FocalLength ^
                            --FieldOfView ^
                            --ThumbnailImage ^
                            --PreviewImage ^
                            --EncodingProcess ^
                            --YCbCrSubSampling ^
                            --Compression "%LOCALOUTNAME%"
if "%ERRORLEVEL%" EQU 1 echo Setting EXIF failed, ignoring

rem There are problems with -delete_original in exiftool, manually remove the file
del "%LOCALOUTNAME%_original"
exit /b 0

:eof
