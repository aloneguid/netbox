#!/bin/bash 

# find all *.cs files and concat them
# (excluding unit tests)

ONAME=NetBox.cs

if [ -f $ONAME ] ; then
	rm $ONAME
	echo "removed $ONAME"
fi

# get all the unique "using" directives, sorted and deduplicated
find . -type f -name "*.cs" ! -iname "*test*" ! -iname "netbox.cs" -exec cat {} + | grep ^using | sort --reverse | uniq > $ONAME

echo "concatenating all .cs files (excluding unit tests) to $ONAME..."
find . -type f -name "*.cs" ! -iname "*test*" ! -iname "netbox.cs" -print | while read filename; do
	echo -e "\n\n// FILE: $filename\n\n"
	# cat "$filename"
	cat "$filename" | grep --invert-match ^using
done >> $ONAME

echo "done."
