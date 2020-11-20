Due to issues with the way that the Database Publish works, Security Policies are not handled correctly. As such it is necessary to run a script to remove the policies and also the views.
This script is in <solution root>\SQL. The file is DropRLSAndViewsSp.sql.
This file cannot be part of the database publish process ie a Pre Deploy script, as it deletes objects that the build already believes exist, so will not replace.
