setlocal

SET PACKAGES=%~dp0..\packages\
SET PROTOC=%PACKAGES%Google.Protobuf.Tools\tools\windows_x64\protoc.exe
SET PLUGIN=%PACKAGES%Grpc.Tools\tools\windows_x64\grpc_csharp_plugin.exe

%PROTOC% -I=EventService.Protocol\proto^
         --csharp_out=EventService.Protocol\proto^
         --grpc_out=EventService.Protocol\proto^
         --plugin=protoc-gen-grpc=%PLUGIN%^
         EventService.Protocol\proto\common.proto

%PROTOC% -I=EventService.Protocol\proto^
         --csharp_out=EventService.Protocol\proto^
         --grpc_out=EventService.Protocol\proto^
         --plugin=protoc-gen-grpc=%PLUGIN%^
         EventService.Protocol\proto\producer.proto