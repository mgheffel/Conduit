#!/usr/local/bin/perl

#Select sequences from a FASTA or FASTQ file based on a list of IDs
##Developed by Reza Mazloom rmazloom@ksu.edu
##version 2.0.0

#Usage:
#perl select_sequences.pl list.txt seqs.fasta output.fasta

use strict;
use warnings;

my %seqs = ();

open (MYFILE1, "$ARGV[0]") or die "Couldn't open: $!";
open (MYFILE2, "$ARGV[1]") or die "Couldn't open: $!";
open (MYFILE3, ">$ARGV[2]") or die "Couldn't open: $!";

while (<MYFILE1>){
	chomp;
	$seqs{"$_"} = 1;
}
close (MYFILE1);

my $flag = 0;
while (<MYFILE2>){
	chomp;
	my $line = $_;
	if ($line =~ /^>([^\s]+)\s.*$/ or $line =~ /^@([^\s]+)\s.*$/){
		$flag = 0;
		if ($seqs{"$1"}){
			print MYFILE3 "$line\n"; 
			$flag = 1;
		}
	}elsif($flag > 0){
		print MYFILE3 "$line\n"; 
	}
}
close (MYFILE2);
close (MYFILE3);

