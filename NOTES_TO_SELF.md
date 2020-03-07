edit nuget package metadata in `src\canopy\canopy.paket.template`
create nuget package with: `.paket\paket.exe pack .`
update the REALEASE_NOTES also so that the version of the assembly and documentation are correct

may need to `paket.exe restore -f` if it says file not found when running .cmd
need to fix paths in docs
manually push document updates from temp/gh-pages, have to add a remote that is ssh not https