using static Utils.Utils;
using FileInfo = Day09.FileInfo;

var inputString = File.ReadAllText("input.txt").Trim();

ExecuteAndMeasure("First part", () => SolvePartOne(inputString));
ExecuteAndMeasure("Second part", () => SolvePartTwo(inputString));

return 0;

static List<int> MapToBlocks(string input)
{
    var blocks = new List<int>();
    var fileId = 0;
    var isFile = true;

    foreach (var c in input)
    {
        var size = c - '0';
        if (isFile)
        {
            blocks.AddRange(Enumerable.Repeat(fileId, size));
            fileId++;
        }
        else
        {
            blocks.AddRange(Enumerable.Repeat(-1, size));
        }

        isFile = !isFile;
    }

    return blocks;
}

static void CompactBlocks(List<int> blocks)
{
    bool movedFilePart;
    do
    {
        movedFilePart = false;
        
        var rightmostFile = blocks.Count - 1;
        while (rightmostFile >= 0 && blocks[rightmostFile] == -1)
        {
            rightmostFile--;
        }
        
        if (rightmostFile <= 0)
            break;
        
        var leftmostSpace = -1;
        for (var i = 0; i < rightmostFile; i++)
        {
            if (blocks[i] != -1) continue;
            
            leftmostSpace = i;
            break;
        }
        
        if (leftmostSpace == -1) continue;
        
        blocks[leftmostSpace] = blocks[rightmostFile];
        blocks[rightmostFile] = -1;
        movedFilePart = true;
    } while (movedFilePart);
}

static List<FileInfo> GetFileInfos(List<int> blocks)
{
    var fileInfos = new List<FileInfo>();
    var currentId = -1;
    var startPos = -1;
    var size = 0;

    for (var i = 0; i < blocks.Count; i++)
    {
        if (blocks[i] == -1) continue;
        
        if (blocks[i] != currentId)
        {
            if (currentId != -1)
            {
                fileInfos.Add(new FileInfo { Id = currentId, Size = size, StartPosition = startPos });
            }
            currentId = blocks[i];
            startPos = i;
            size = 1;
        }
        else
        {
            size++;
        }
    }
    
    if (currentId != -1)
    {
        fileInfos.Add(new FileInfo { Id = currentId, Size = size, StartPosition = startPos });
    }
    
    return fileInfos;
}

void CompactWholeFiles(List<int> blocks)
{
    var fileInfos = GetFileInfos(blocks);
    
    fileInfos.Sort((a, b) => b.Id.CompareTo(a.Id));
    
    foreach (var file in fileInfos)
    {
        var bestPosition = -1;
        var currentSpanStart = -1;
        var currentSpanSize = 0;
        
        for (var i = 0; i < file.StartPosition; i++)
        {
            if (blocks[i] == -1)
            {
                if (currentSpanStart == -1)
                {
                    currentSpanStart = i;
                }
                currentSpanSize++;

                if (currentSpanSize < file.Size) continue;
                
                bestPosition = currentSpanStart;
                break;
            }

            currentSpanStart = -1;
            currentSpanSize = 0;
        }

        if (bestPosition == -1) continue;
        
        for (var i = 0; i < file.Size; i++)
        {
            blocks[file.StartPosition + i] = -1;
        }
        
        for (var i = 0; i < file.Size; i++)
        {
            blocks[bestPosition + i] = file.Id;
        }
    }
}

long CalculateChecksum(List<int> blocks)
{
    long checksum = 0;
    for (var pos = 0; pos < blocks.Count; pos++)
    {
        if (blocks[pos] != -1)
        {
            checksum += (long)pos * blocks[pos];
        }
    }
    return checksum;
}

long SolvePartOne(string input)
{
    var blocks = MapToBlocks(input);
    
    CompactBlocks(blocks);
    
    return CalculateChecksum(blocks);
}

long SolvePartTwo(string input)
{
    var blocks = MapToBlocks(input);
    
    CompactWholeFiles(blocks);

    return CalculateChecksum(blocks);
}
