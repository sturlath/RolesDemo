# RolesDemo
To help with [this support question](https://support.abp.io/QA/Questions/1713/Administration-menu-missing-after-update-to-440#answer-f169cd5a-634e-9a61-9246-39fe74af474b)

# Steps
1. Run the DbMigrator and see [roles beeing seeded](https://github.com/sturlath/RolesDemo/blob/master/src/RolesDemo.DbMigrator/DataSeed/RoleDataSeedContributor.cs)
   * For some reason I get an error when seeding in the [RegisteredUserHandler](https://github.com/sturlath/RolesDemo/blob/16eab8e3e20ef7036c4caa5119a4d426b87ed476/src/RolesDemo.Domain/Users/RegisteredUserHandler.cs#L119)
2. With this demo app I don´t have the "add tenant" UI so I can´t add a tenant that way so no such code.
   * I also can´t seem to debug into the [HandleEventAsync](https://github.com/sturlath/RolesDemo/blob/16eab8e3e20ef7036c4caa5119a4d426b87ed476/src/RolesDemo.Domain/Users/RegisteredUserHandler.cs#L51) in this demo

I hope you can make sense out of this all

