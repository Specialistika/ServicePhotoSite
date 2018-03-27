function UploadFileChunk(Chunk, FileName, DirChunk) {
    var FD = new FormData();
    FD.append('file', Chunk, FileName);
    $.ajax({
        url: '/Native/UploadDir',
        type: "GET",
        data: { DirChunk: DirChunk },
        success: function (responce) {
            if (responce.pathDirectory != null) {
                FD.append(responce.pathDirectory, String)
                $.ajax({
                    type: "POST",
                    url: '/Native/UploadFileChuck',
                    contentType: false,
                    processData: false,
                    data: FD,
                    success: function (FD) {
                        if(FD.StatusCode = 200)
                        {
                            httpResponce("Передача данных успешна")
                        }
                        else
                        {
                            httpResponce("Ошибка при передаче данных")
                        }
                        if (FD.Content != null)
                        {
                            httpResponceChunk(FD.Content)
                        }
                    }
                });
            }
        }
    });
}

function UploadFile(TargetFile, DirChunk) {
    var FileChunk = [];
    var file = TargetFile[0];
    var MaxFileSizeMB = 2;
    var BufferChunkSize = MaxFileSizeMB * (1024 * 1024);
    var ReadBuffer_Size = 2048;
    var FileStreamPos = 0;
    var EndPos = BufferChunkSize;
    var Size = file.size;

    while (FileStreamPos < Size) {
        FileChunk.push(file.slice(FileStreamPos, EndPos));
        FileStreamPos = EndPos; // jump by the amount read
        EndPos = FileStreamPos + BufferChunkSize; // set next chunk length
    }
    var TotalParts = FileChunk.length;
    var PartCount = 0;
    while (chunk = FileChunk.shift()) {
        PartCount++;
        var FilePartName = file.name + ".part_" + PartCount + "." + TotalParts;
        move(TotalParts, PartCount);
        UploadFileChunk(chunk, FilePartName, DirChunk);
    }
}
