# Replaceator

Replaceator is a tool which goal is to automate some tedious copy pasting I had to do at my current job.
I had a text file with a bunch if text in it (a SQL trigger), and I had to change two words in the file for 30 times.
So I though I'd automate it. So I built this, and it took me way more time than 30 copy paste, but you know, it does more and it could be usefull later of for somebody else.

```
./Replaceator.exe --help
```
```
Replaceator 1.0.0
Copyright (C) 2020 Replaceator

  -t, --template     Required. Template file that contains the pattern word

  -p, --pattern      Required. Pattern in template file to replace by replace words

  -r, --replace      Required. Replace words that will be put in place of the pattern. It can also be a list of existing
                     files. If you pass files, a replace word will be a line in these files

  -e, --extension    Extension of the output file (defaults to 3 random characters)

  -o, --output       Output for the replacement. The default is the current directory (usually exe's directory). It can
                     point to
                      - A directory     => A file with a random name will be generated for each replace word
                      - A file  => The file will be erased and the content will become the content of the template file
                      times the replace words (append mode)

  --help             Display this help screen.

  --version          Display version information.
  ```

## Exemple

Let's say you want a personalized invite for your birthday to your 400 Facebook friends.
You create the following template.txt file:
Hello, you, <thenameoftheinvitee>, are invited to my birthday party (if you can afford an expensive and personalized gift for me of course).

```
./Replaceator.exe -t "template.txt" -p "<thenameoftheinvitee>" -e "txt" -r "John Doe" "Jon Skeet" ...
```

Alternatively you can use a friends.txt file containing the name of all your friends like that:

```
John Doe
Jon Skeet
```

And use the following command:
```
./Replaceator.exe -t "template.txt" -p "<thenameoftheinvitee>" -e "txt" -r "friends.txt"
```

## Running the tests

```
dotnet test
```

## Publish to a portable executable

```
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true
```