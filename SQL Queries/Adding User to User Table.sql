USE Anoroc

insert into dbo.Users (Email, Firebase_Token, AccessToken,FirstName, UserSurname, carrierStatus,loggedInAnoroc,loggedInFacebook,loggedInGoogle,currentlyLoggedIn)
VALUES ('test@gmail.com','df965K30qpA:APA91bG170UHBKCVKorN7Eb-H9GXFTzYl1vlbn6F60dJjCccm6z6LfmGHVYduKZjEXJd-THer9JcGkxw5sCw9Na4RuJJOHlZPkxDCUOOu9kmzYX-bCVwLN2Uw3qF-rgNK5VL9kbLRzp_','thisisatoken','Testy','McTestyFace',0,0,0,0,1)
GO