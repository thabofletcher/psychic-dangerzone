$(function () {
    var commandIndex = 0;
    var fullBuffer = "";
    var buffer = "";
    commandHistory = [];

    var flush = function () {
        $('#console')[0].innerHTML = fullBuffer;
    }

    var cancel = function () {
        if (buffer.length > 0)
            fullBuffer = fullBuffer.slice(0, -buffer.length);
        buffer = "";
    }

    var write = function (output) {
        fullBuffer += output;
    }

    var writeToCommand = function (command) {
        buffer += command;
        write(command);
    }

    var prompt = function () {
        write("&gt;&gt;&gt;&nbsp;&#127;");
    }

    var backspace = function () {
        fullBuffer = fullBuffer.slice(0, -1);
        buffer = buffer.slice(0, -1);
        flush();
    }

    $('#console').keypress(function (event) {
        var charText = String.fromCharCode(event.which);

        if (event.which == 13)
            submit(buffer);
        else
            writeToCommand(charText);

        flush();

        event.preventDefault();
        return false;
    });

    var ctrl_set = false;
    $('#console').keydown(function (event) {
        if (event.which == 8) {
            var charText = String.fromCharCode(event.which);
            if ($('#console').text().substr($('#console').getCaret() - 1, 1).charCodeAt(0) != 127) {
                backspace();
            }

            event.preventDefault();
            return false;
        }

        if (event.which == 38) {
            if (commandIndex > 0) {
                cancel();
                commandIndex--;
                writeToCommand(commandHistory[commandIndex]);
                flush();
            }

            event.preventDefault();
            return false;
        }

        if (event.which == 40) {
            if (commandIndex < commandHistory.length - 1) {
                cancel();
                commandIndex++;
                writeToCommand(commandHistory[commandIndex]);
                flush();
            }

            event.preventDefault();
            return false;
        }

        //ctrl-x
        // ctrl-x locks up the output of the textarea or this might be cooler $('#console').getCaret() < $('#console').text().lastIndexOf(String.fromCharCode(127))
        if (ctrl_set && event.which == 88) {
            event.preventDefault();
            return false;
        }

        if (event.which == 17)
            ctrl_set = true;

    });

    var submit = function (command) {
        if (command != "") {
            commandHistory.push(command);
            commandIndex = commandHistory.length;

            var enter = String.fromCharCode(13);
            if (command == "clear") {
                fullBuffer = "";
                endCommand();
                return;
            }

            $.post('/home/console', "command=" + command, function (data, textStatus) {
                write(enter + data + enter);
                endCommand();
            }).error(function (xmlreq, text) {
                write(enter + xmlreq.status + " - " + text + enter);
                endCommand();
            });
        }
    }

    var endCommand = function (command) {
        prompt();
        buffer = "";
        flush();
    }

    prompt();
    flush();

    $('#console').focus();
    $('#console').caretToEnd();
});