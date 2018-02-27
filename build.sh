#!/bin/bash

mono .paket/paket.exe restore -v
exit_code=$?
if [ $exit_code -ne 0 ]; then
	exit $exit_code
fi

#workaround assembly resolution issues in build.fsx
export FSHARPI=`which fsharpi`
cat - > fsharpi <<"EOF"
#!/bin/bash
libdir=$PWD/packages/FAKE/tools/
$FSHARPI --lib:$libdir $@
EOF
chmod +x fsharpi
mono packages/FAKE/tools/FAKE.exe Build.fsx $@
rm fsharpi
