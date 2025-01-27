using System; 
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class variable{
	public static Dictionary<string, pair<double,string>> dic = new Dictionary<string, pair<double,string>>();
	public static int max_block_num = 0;
	
	public static string name(string name, int block){//変数名検索
		while(block >= 0){
			if(dic.ContainsKey(block.ToString() + name)){
				return block.ToString() + name;
			}
			block--;
		}
		return "";
	}

	public static string formula(int block,string[] words,int n,int length){//式作成
		string ret = "";
        for(int j = n; j < length; j++){
        	string var_name = name(words[j],block);
        	
			if(var_name != ""){
				pair<double,string> r = read(var_name);
				if(r.second == "double") ret += r.first.ToString("F10");
				else ret += ((int)(r.first)).ToString();
			}else{
            	ret += words[j];
			}
        }
        return ret;
	}
	
	public static pair<double,string> read(string var_name){//変数の値を返却
		return dic[var_name];
	}
	
	public static void sub(string var_name,double num){//変数に代入
		dic[var_name] = new pair<double,string>(num,dic[var_name].second);
	}

	public static void make(int block,string[] words){//変数の宣言
		string s = "",name = "";
		bool flag = false;
		for(int j = 1; j<words.Length; j++){
			if(words[j] != ""){
				if(flag == false){
					name = words[j];
					flag = true;
					s = "";
				}else if(words[j] == "," || words[j] == ";"){
					if(s != ""){
						string[] ss = s.Split(' ');
						pair<double,string> tmp = new pair<double,string>(new Parser(formula(block,ss,0,ss.Length)).expr().first,words[0]);
						dic.Add( block.ToString() + name,tmp);
						if(block > max_block_num)max_block_num = block;		
					}else{
						dic.Add( block.ToString() + name,new pair<double,string>(0,words[0]));
						if(block > max_block_num)max_block_num = block;
					}
					s = "";
					flag = false;
				}else{
					if(words[j] != "=")s += words[j] + " ";
				}
			}
		}
	}
	
	public static void del(int block){//不要な変数の削除
		while(block <= max_block_num){
			string del = max_block_num.ToString();
			var del_list = new List<string>();
				
			foreach (string key in dic.Keys) {
				if(key.IndexOf(del) == 0){
					del_list.Add(key);
				}
        	}
        	foreach (string key in del_list)dic.Remove(key);
        	max_block_num--;
        }
	}	
}