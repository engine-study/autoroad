<?php  

	//we know this is an extremely vulnerable way of saving the map file
	//if you want to help secure this please email info@scrnprnt.ca to help
	//otherwise we are trusting our community to not tamper with the map save
	//thanks 


if(isset($_POST['fileName']) && isset($_POST['data']) && isset($_POST['pleasedonthurtus']) ) {
    
    $target_file = basename($_POST['fileName']);


    $nohurt = $_POST['pleasedonthurtus'];
    $nohurtbyte = base64_decode($nohurt);
    
    
    $ivByte64 = base64_encode(substr($nohurtbyte, 0, 16));
    $nohurt64 = base64_encode(substr($nohurtbyte, 16, strlen($nohurtbyte) - 16));

    //$nohurtByte = unpack('C*', $nohurt);
    //$ivByte64 = array_map("chr", array_slice($nohurtByte, 0, 16));
    //$nohurt64 = array_map("chr", array_slice($nohurtByte, 16, count($nohurtByte) - 16));
    
    
    $password = 'q2FM203mWDOWEk393mkwWFPWE3025921';
    

    // CBC has an IV and thus needs randomness every time a message is encrypted
    $method = 'aes-256-cbc';

    // Must be exact 32 chars (256 bit)
    // You must store this secret random key in a safe place of your system.
    //$key = $password; //substr(hash('sha256', $password), 0, 32);
    $key = substr(hash('sha256', $password, true), 0, 32);
    //$key = $password;
    // Most secure key
    //$key = openssl_random_pseudo_bytes(openssl_cipher_iv_length($method));

    // IV must be exact 16 chars (128 bit)
    //$iv = chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0) . chr(0x0);
    //$iv = unpack('C*', $plaintext);
    //$iv = substr($nohurt, 0, 16);
    
    $iv = base64_decode($ivByte64);
    $message = base64_decode($nohurt64);
    
    // Most secure iv
    // Never ever use iv=0 in real life. Better use this iv:
    // $ivlen = openssl_cipher_iv_length($method);
    // $iv = openssl_random_pseudo_bytes($ivlen);

    // av3DYGLkwBsErphcyYp+imUW4QKs19hUnFyyYcXwURU=
    $encrypted = base64_encode(openssl_encrypt($target_file, $method, $key, OPENSSL_RAW_DATA, $iv));

    // My secret message 1234
    //$decrypted = openssl_decrypt(base64_decode($encrypted), $method, $key, OPENSSL_RAW_DATA, $iv);
    $fakedecrypted = openssl_decrypt(base64_decode($encrypted), $method, $key, OPENSSL_RAW_DATA, $iv);

    $decrypted = openssl_decrypt($message, $method, $key, OPENSSL_RAW_DATA, $iv);
    //$decrypted = base64_encode($decrypted);

    //NEVER UNCOMMENT THESE ECHOES ELSE THEY LEARN THE KEYS
    /*
    echo 'address=' . $target_file . "\n";
    
    echo 'nohurt=' . $nohurt . "\n";
    echo 'nohurt64=' . $nohurt64 . "\n";
    
    //echo 'IV=' . $iv . "\n";
    echo 'IV64=' . $ivByte64 . "\n";
    
    
    echo 'password=' . $password . "\n";
    echo 'key=' . base64_encode($key) . "\n";
    echo 'cipher=' . $method . "\n\n";
    echo 'encrypted to: ' . $encrypted . "\n\n";
    echo 'fake decrypted to: ' . $fakedecrypted . "\n\n";
    echo 'decrypted to: ' . $decrypted . "\n\n";
    */

    if(strlen($target_file) > 0 && strlen($nohurt) > 0 && $target_file == $decrypted) {

        //get the file basename

        $uploadOk = 1;

        // Check file size
        $size = (int) $_SERVER['CONTENT_LENGTH'];
        if ($size > 500000) {
            echo "bad bad";
            $uploadOk = 0;
        }

        if($uploadOk == 1) {

            $file = $target_file . '.txt';
            file_put_contents($file, $_POST['data']);      
            echo "ok";

        }
    } else {
        echo "not good";
    }


} else {
    echo "missing something";
}  

?>


